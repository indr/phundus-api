﻿using System;
using System.Collections.Generic;
using System.Linq;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Assembler
{
    public class ArticleAssembler
    {
        /// <summary>
        /// Wandelt das übergebene Article-DTO-Objekt in ein neues
        /// Article-Domain-Objekt um.
        /// </summary>
        /// <param name="subject">Das zu assemblierende DTO-Objekt</param>
        /// <returns></returns>
        public static Article CreateDomainObject(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new Article();
            WriteProperties(subject, result);
            CreateChildren(subject, result);
            return result;
        }

        /// <summary>
        /// Assembliert das übergebene DTO in das korrespondierende Domain-Object,
        /// welches zuerst aus dem Repository geladen wird.
        /// </summary>
        /// <param name="subject">Das zu assemblierende DTO.</param>
        /// <returns></returns>
        public static Article UpdateDomainObject(ArticleDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = IoC.Resolve<IArticleRepository>().Get(subject.Id);
            Guard.Against<EntityNotFoundException>(result == null, "Article entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            WriteProperties(subject, result);
            UpdateChildren(subject, result);
            return result;
        }

        /// <summary>
        /// Assembliert die Properties des übergebenen DTOs in das übergebene Domain-Object.
        /// </summary>
        /// <param name="subject">Das zu assemblierende DTO.</param>
        /// <param name="result">Das zu aktualisierende Domain-Object.</param>
        private static void WriteProperties(BasePropertiesDto subject, FieldedEntity result)
        {
            // Neue Properties hinzufügen, oder bestehende Property-Values aktualisieren.
            var propertyDefinitionRepo = IoC.Resolve<IFieldDefinitionRepository>();
            foreach (var each in subject.Properties)
            {
                FieldValue propertyValue = null;
                if (result.HasField(each.PropertyId))
                {
                    propertyValue = result.SetFieldValue(each.PropertyId, each.Value);
                }
                else
                    propertyValue = result.AddField(propertyDefinitionRepo.Get(each.PropertyId), each.Value);

                propertyValue.IsDiscriminator = each.IsDiscriminator;
            }

            // Properties, die nicht mehr im DTO vorhanden sind, entfernen.
            var propertiesToRemove = new List<FieldValue>();
            foreach (var each in result.FieldValues)
            {
                if (subject.Properties.FirstOrDefault(x => x.PropertyId == each.FieldDefinition.Id) == null)
                    propertiesToRemove.Add(each);
            }
            foreach (var each in propertiesToRemove)
                result.RemoveField(each.FieldDefinition);
        }

        private static void CreateChildren(ArticleDto subject, Article result)
        {
            foreach (var each in subject.Children)
            {
                var child = new Article();
                WriteProperties(each, child);
                result.AddChild(child);
            }
        }

        private static void UpdateChildren(ArticleDto subject, Article result)
        {
            // Neue Childs hinzufügen, oder bestehende Updaten
            foreach (var each in subject.Children)
            {
                Article child = null;
                if (each.Id > 0)
                    child = UpdateDomainObject(each);
                else
                    child = CreateDomainObject(each);
                result.AddChild(child);
            }

            // Children, die nicht mehr im DTO vorhanden sind, entfernen.
            var childrenToRemove = new List<CompositeEntity>();
            foreach (var each in result.Children)
            {
                if (subject.Children.FirstOrDefault(x => x.Id == each.Id) == null)
                    childrenToRemove.Add(each);
            }
            foreach (var each in childrenToRemove)
                result.RemoveChild(each);
        }
    }
}