using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace phiNdus.fundus.Core.Web.UnitTests {
    internal static class ModelValidator {

        /// <summary>
        /// Validiert das übergebene Model gemäss den definierten Annotations und löst
        /// ggf. eine <see cref="System.ComponentModel.DataAnnotations.ValidationException"/> aus.
        /// </summary>
        /// <typeparam name="TModel">Der Typ des zu prüfenden Models. Dieser Typ wird als Basis
        /// für das ermitteln der Properties verwendet. Es kann also auch ein Basistyp des zu prüfenden
        /// Models sein, was für einen Sinn das auch immer machen mag.</typeparam>
        /// <param name="model">Die zu prüfende Modelinstanz.</param>
        internal static void Validate<TModel>(TModel model) {
            var modelType = typeof(TModel);
            var propertyInfos = modelType.GetProperties();

            foreach (var propertyInfo in propertyInfos) {
                var propertyValue = propertyInfo.GetValue(model, null);
                var attributes = propertyInfo.GetCustomAttributes(false).OfType<ValidationAttribute>();

                foreach (var attribute in attributes) {
                    attribute.Validate(propertyValue, propertyInfo.Name);
                }
            }
        }
    }
}
