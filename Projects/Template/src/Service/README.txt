In order to add a new Model class:
1- Create the model in model folder
2- If the mapping for the model will be non standard, create a configuration file under database\mappings
3- Create or update the appropriate Context file (representing the database to connect to)
	a- It needs to extend DbContext and IValidatedContext
	b- include the DbSet line (i.e. public DbSet<Publication> Publications { get; set; })
	c- update the Support method
	d- update the GetSet method
	e- update config files to include the connection string if a new context was created
4- Update the EFRegistry
	a- Add creation of context if new one was created (i.e. For<ASIInternetContext>().HybridHttpOrThreadLocalScoped().Use<ASIInternetContext>();)
	b- Add creation of the repository with the appropriate context
5- if specialied business logic method need to be added, they will need to be added to ObjectService class