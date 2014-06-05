using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services.PersonifyProxy
{
    public static class MemberTypeIDToCD
    {
        public static readonly IDictionary<int, MemberData> Data;

        static MemberTypeIDToCD()
        {
            Data = new Dictionary<int, MemberData>()
            {
                {3, new MemberData() {MemberTypeID=3, MemberTypeCD="DECORATOR",MemberStatusClass="ACTIVE"}},
                {12, new MemberData() {MemberTypeID=12, MemberTypeCD="DECORATOR",MemberStatusClass="LEAD"}},
                {9, new MemberData() {MemberTypeID=9, MemberTypeCD="END_BUYER",MemberStatusClass="NON_MEMBER"}},
                {2, new MemberData() {MemberTypeID=2, MemberTypeCD="SUPPLIER",MemberStatusClass="ACTIVE"}},
                {14, new MemberData() {MemberTypeID=14, MemberTypeCD="UNKNOWN",MemberStatusClass="INDIV"}},
                {6, new MemberData() {MemberTypeID=6, MemberTypeCD="DISTRIBUTOR",MemberStatusClass="LEAD"}},
                {16, new MemberData() {MemberTypeID=16, MemberTypeCD="DISTRIBUTOR",MemberStatusClass="ACTIVE"}},
                {11, new MemberData() {MemberTypeID=11, MemberTypeCD="UNKNOWN (MEDIA)",MemberStatusClass="NON_MEMBER"}},
                {1, new MemberData() {MemberTypeID=1, MemberTypeCD="DISTRIBUTOR",MemberStatusClass="ACTIVE"}},
                {13, new MemberData() {MemberTypeID=13, MemberTypeCD="AFFILIATE",MemberStatusClass="NON_MEMBER"}},
                {7, new MemberData() {MemberTypeID=7, MemberTypeCD="SUPPLIER",MemberStatusClass="LEAD"}},
                {10, new MemberData() {MemberTypeID=10, MemberTypeCD="UNKNOWN",MemberStatusClass="INDIV"}},
                {15, new MemberData() {MemberTypeID=15, MemberTypeCD="UNKNOWN",MemberStatusClass="NON_MEMBER"}},
            };
        }
    }
}
