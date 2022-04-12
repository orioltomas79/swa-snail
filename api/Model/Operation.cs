using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using Newtonsoft.Json;

namespace api.Model
{
    public class Operation
    {
        /// <summary>
        /// Gets or sets the operation ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}


