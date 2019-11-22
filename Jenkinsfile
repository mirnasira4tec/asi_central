import org.jenkinsci.plugins.workflow.cps.CpsThread
import org.jenkinsci.plugins.workflow.actions.LabelAction
import com.cwctravel.hudson.plugins.extended_choice_parameter.ExtendedChoiceParameterDefinition

/**
 * JOB CONFIGURATION
 */
def adminSolutionName = 'Admin Tool.sln'
def packageNames = []

def octopusServer = 'http://asi-deploy-02/'
def octopusProject = 'asicentraladmin.asinetwork.local-new'
def octopusVersion = "1.2.${env.BUILD_NUMBER}"
def octopusEnvironments = ['STAGE-ASICentral-Family', 'UAT-ASICentral-Family', 'PROD-ASI Central-Family']
def defaultDeployTo = ['STAGE-ASICentral-Family']
def nugetServer = "\\\\asi-nuget-01\\Packages"
def buildTarget = '\\\\asi-nas-01\\jenkins\\Releases'

def deployToParameter = new ExtendedChoiceParameterDefinition("deployTo", "PT_CHECKBOX",
    octopusEnvironments.join(','), "", "", "", "", "", "", "", // source of value
    defaultDeployTo.join(','), "", "", "", "", "", "", "", // source of default value
    "", "", "", "", "", "", "", "", // source of value description
    false, false, octopusEnvironments.size(), "Environment(s) to deploy to after the release is created.", ",")

def buildOptionsParameter = ['Build Only', 'Publish Contracts', 'Publish Deployment Packages', 'Create Release']

if (packageNames.size() == 0) {
    buildOptionsParameter.remove(1)
}

def projectProperties = [
    //disableConcurrentBuilds(),
    parameters([
        choice(choices: "Release\nDebug\n", description: 'Solution configuration used for build.', name: 'BUILDCONFIGURATION'),
        string(defaultValue: buildTarget, description: 'The target location for the distribution package', name: 'BUILDTARGETDIR'),
        choice(choices: buildOptionsParameter.join('\n'), description: 'Build/deployment options', name: 'buildOptions'),
        deployToParameter,
		choice(choices: "Hotfix\nFull\n", description: 'Choose deployment type', name: 'DeploymentType'),
		string(defaultValue: "False", description: 'True or False', name: 'tobeRecycled')
    ]),
    [$class: 'BuildDiscarderProperty', strategy: [$class: 'LogRotator', numToKeepStr: '10']]
]

properties(projectProperties)

def buildConfiguration = 'Release'
def createArtifacts = false
def createRelease = false
def createNugetPackages = false
def deploymentType = 'Hotfix'
def tobeRecycled = 'False'

if (params.BUILDCONFIGURATION != null) {
    buildConfiguration = params.BUILDCONFIGURATION
}

if (params.BUILDTARGETDIR != null) {
    buildTarget = params.BUILDTARGETDIR
}

if (params.DeploymentType != null) {
    deploymentType = params.DeploymentType
}

if (params.tobeRecycled != null) {
    tobeRecycled = params.tobeRecycled
}

if (params.buildOptions != null) {
    if (params.buildOptions == 'Publish Contracts') {
        createNugetPackages = true
    } else {
        createRelease = params.buildOptions == 'Create Release'
        createArtifacts = createRelease || params.buildOptions == 'Publish Deployment Packages'
    }
	println("Params: Build Options: '${params.buildOptions}'")
}

def deploymentTargets = ""

if (params.deployTo != null && createRelease) {
    deploymentTargets = params.deployTo.tokenize(",")
}

@NonCPS // https://stackoverflow.com/questions/39795652/error-with-changeset-in-jenkins-pipeline-errorjava-io-notserializableexception
def getReleaseNotes() {
    def notes = "###Commits:";
    // https://support.cloudbees.com/hc/en-us/articles/217630098-How-to-access-Changelogs-in-a-Pipeline-Job-
    def changeLogSets = currentBuild.changeSets
    for (int i = 0; i < changeLogSets.size(); i++) {
        def entries = changeLogSets[i].items
        for (int j = 0; j < entries.length; j++) {
            def entry = entries[j]
            notes += "\n\n* ${entry.msg} by ${entry.author} on ${new Date(entry.timestamp)} [View on GitHub](https://github.com/asi/asi_central/commit/${entry.commitId})"
        }
    }

    return notes;
}

node {
    def startTime = new Date();

    def buildNumber = env.BUILD_NUMBER
    def workspace = env.WORKSPACE
    def buildUrl = env.BUILD_URL
    def jobName = "${env.JOB_NAME}".tokenize('/')
    def jobOrg = jobName[0]
    def jobRepo = jobName[1]
    def jobBranch = jobName[2]

    def nuget = "\"${workspace}\\.nuget\\nuget.exe\""

    // PRINT ENVIRONMENT TO JOB
    echo "workspace directory is $workspace"
    echo "build URL is $buildUrl"
    echo "build Number is $buildNumber"
    echo "PATH is $env.PATH"
    echo "Branch is $jobBranch"    

    if (jobBranch != "master") {
        octopusVersion = "1.2.${env.BUILD_NUMBER}-${jobBranch}"
    }

    try {
        def unstableReason = '';
        def artifactStr = createArtifacts ? "Artifacts enabled" : ""
 
        slackSend message: "Build Started - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>) ${artifactStr}"
    
        stage('Checkout') {
            step([$class: 'GitHubSetCommitStatusBuilder'])
            
			bat "git config --global core.longpaths true"

			checkout([$class: 'GitSCM',
				branches: scm.branches,
				extensions: scm.extensions + [
					[$class: 'SubmoduleOption', recursiveSubmodules: true]
				],
				userRemoteConfigs: scm.userRemoteConfigs
			])

            echo getReleaseNotes();
        }
        
        stage('Pre-build') {
            def preBuildSteps = [:]
            
            preBuildSteps["Nuget Restore"] = {
                bat("${nuget} restore \"${adminSolutionName}\"")
            };

            parallel(preBuildSteps)
        }
		
        stage('Build') {
            def buildSteps = [:]
            
            buildSteps["Build Solution"] = {
                withCredentials([string(credentialsId: 'octopus-api-key', variable: 'octoApiKey')]) {
                    bat "\"${tool 'MSBuild2017'}\" asi_central.xml /t:Compile /p:RunOctoPack=${createArtifacts} /p:OctoPackPackageVersion=${octopusVersion} /p:OctoPackPublishPackageToFileShare=${buildTarget} /p:OctoPackPublishPackageToHttp=${octopusServer}nuget/packages /p:OctoPackPublishApiKey=${octoApiKey}"
                }
            };

            parallel(buildSteps)
        }
		
	def testFiles = [:]

        stage('Post-build') {
            bat "if not exist \"reports\\target\" mkdir \"reports\\target\""

            def testFileSteps = [:]
            // testFiles = findFiles(glob: "**/ASI.*.Tests.Internal.dll")
            for (int i = 0; i < testFiles.length; i++) {
                def testFile = testFiles[i]
                def testName = testFile.name.replace('.dll','')
                testFileSteps[testName] = {
                    try {
                        bat "\"C:\\bin\\nunit3\\nunit3-console.exe\" \"${testFile.path}\" --result=\"reports\\target\\${testFile.name}-result.xml\""
                    } finally {
                        // WORKAROUND: https://issues.jenkins-ci.org/browse/JENKINS-38442
                        CpsThread.current().head.get().addAction(new LabelAction("Test:${testName}"))
                    }
                };
            }

            parallel(testFileSteps)
        }
        
        stage('Artifacts') {
            def buildTargets = [:]

            if ( testFiles.length > 0) {
                buildTargets["Test Results"] = {
                    nunit testResultsPattern: 'reports/target/*-result.xml'

                    dir('reports') {
                        deleteDir()
                    }
                }
            }

            parallel(buildTargets)
        }
        
         stage('Deploy') {
            def deploySteps = [:]

	    if (createRelease) {
               deploySteps["Create Release"] = {
                   def releaseNotes = "###${jobRepo} ([${jobBranch}](${env.JOB_URL}))\n\nBuild [#${env.BUILD_NUMBER}](${env.BUILD_URL})\n\n" + getReleaseNotes()

                    writeFile file: "releaseNotes.md", text: releaseNotes
                    if (createArtifacts) {
                        withCredentials([string(credentialsId: 'octopus-api-key', variable: 'octoApiKey')]) {
                            bat("octo create-release --server ${octopusServer} --apiKey ${octoApiKey} --project \"${octopusProject}\" --packageversion \"${octopusVersion}\" --version \"${octopusVersion}\" --releasenotesfile \"releaseNotes.md\"")
                            for (int i = 0; i < deploymentTargets.size(); i++) {
                                bat("octo deploy-release --server ${octopusServer} --apiKey ${octoApiKey} --project \"${octopusProject}\" --releaseNumber \"${octopusVersion}\" --deployto \"${deploymentTargets[i]}\" --variable=\"DeploymentType:${deploymentType}\" --variable=\"tobeRecycled:${tobeRecycled}\"")
                            }
                        }
                    }
                }
            }

            parallel(deploySteps)
        }
        
     
        if(currentBuild.result == 'UNSTABLE') {
            def message = "Build Completed as unstable - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
            
            if(unstableReason != '') {
                message = message + ' Reason: ' + unstableReason
            }

            slackSend color: 'warning', message: message
        } else {
            slackSend color: 'good', message: "Build Completed - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
        }
    } catch (error) {
        slackSend color: 'danger', message: "Build Failed - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
        
        throw error
    } finally {
        stage('Cleanup') {
            step([$class: 'WsCleanup', notFailBuild: true])
        }
    }
}
