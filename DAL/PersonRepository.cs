﻿using DAL.Models;
using Linq.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DAL
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AdventureWorks2019Context context;
        public PersonRepository(AdventureWorks2019Context context)
        {
            this.context = context;
        }


        public async Task<IEnumerable> GetAllPerson(PersonFilter filter)
        {
            var query = await context.People.Skip((filter.pageNo - 1) * filter.itemsPerPage).Take(filter.itemsPerPage).Join(context.PersonPhones, p => p.BusinessEntityId, pp => pp.BusinessEntityId, (p, pp) => new { p, pp }).Select(result => new
            {
                BusinessEntityId = result.p.BusinessEntityId,
                PersonType = result.p.PersonType,
                NameStyle = result.p.NameStyle,
                Title = result.p.Title,
                FirstName = result.p.FirstName,
                MiddleName = result.p.MiddleName,
                LastName = result.p.LastName,
                Suffix = result.p.Suffix,
                EmailPromotion = result.p.EmailPromotion,
                AdditionalContactInfo = result.p.AdditionalContactInfo,
                Demographics = result.p.Demographics,
                rowguid = result.p.Rowguid,
                ModifiedDate = result.p.ModifiedDate,
                PhoneNumber = result.pp.PhoneNumber
            }).ToListAsync();
            return query;
        }

        public async Task<IEnumerable> GetAllPersonQuerySyntax(PersonFilter filter)
        {
            var query = (from p in context.People
                         join pp in context.PersonPhones on p.BusinessEntityId equals pp.BusinessEntityId into lj
                         from pn in lj.DefaultIfEmpty()
                         select new
                         {
                             BusinessEntityId = p.BusinessEntityId,
                             PersonType = p.PersonType,
                             NameStyle = p.NameStyle,
                             Title = p.Title,
                             FirstName = p.FirstName,
                             MiddleName = p.MiddleName,
                             LastName = p.LastName,
                             Suffix = p.Suffix,
                             EmailPromotion = p.EmailPromotion,
                             AdditionalContactInfo = p.AdditionalContactInfo,
                             Demographics = p.Demographics,
                             rowguid = p.Rowguid,
                             ModifiedDate = p.ModifiedDate,
                             PhoneNumber = pn == null ? null : pn.PhoneNumber,
                         });
            if (filter.IsAscending)
            {
                switch (filter.sortBy)
                {
                    case "FirstName":
                        query = query.OrderBy(x => x.FirstName);
                        break;
                    case "LastName":
                        query = query.OrderBy(x => x.LastName);
                        break;
                    default:
                        query = query.OrderBy(x => x.BusinessEntityId);
                        break;
                }
            }
            else
            {
                switch (filter.sortBy)
                {
                    case "FirstName":
                        query = query.OrderByDescending(x => x.FirstName);
                        break;
                    case "LastName":
                        query = query.OrderByDescending(x => x.LastName);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.BusinessEntityId);
                        break;
                }
            }
            return query.Skip((filter.pageNo - 1) * filter.itemsPerPage).Take(filter.itemsPerPage).ToList();
        }

        public async Task<IEnumerable> GetAllPersonSqlQuery(PersonFilter filter)
        {
            var query = "select * from Person left join PersonPhone on Person.BusinessEntityId=PersonPhone.BusinessEntityId";
            return query;
        }
    }
}
