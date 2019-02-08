// Entity.cs
// Created By: Adam Renaud
// Created on: 2019-01-06

using System;
using System.Collections.Generic;

using DxfLibrary.Parse;
using DxfLibrary.Parse.Sections;
using DxfLibrary.Geometry;
using DxfLibrary.Utilities;

namespace DxfLibrary.Entities
{
    /// <summary>
    /// Entity Base class, defines the basic
    /// Functions for an entity
    /// </summary>
    public class Entity : IEntity
    {
        #region Protected Members

        /// <summary>
        /// The entity geometric base
        /// </summary>
        protected GeoBase _geometricBase;

        #endregion

        #region Public Members

        /// <summary>
        /// The type of the Entity
        /// </summary>
        public Type EntityType {get; set;}

        /// <summary>
        /// The Entity's Handle
        /// </summary>
        public string Handle {get; set;}

        /// <summary>
        /// Objects that this entity references
        /// </summary>
        public List<IEntityReference> References {get;}

        /// <summary>
        /// The Entity Layer name
        /// </summary>
        public string LayerName {get; set;}

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Entity()
        {

        }

        /// <summary>
        /// Internal Constructor from a struct
        /// </summary>
        /// <param name="struc"></param>
        internal Entity(IEntityStruct struc)
        {
            References = struc.References;
            EntityType = struc.EntityType;
            Handle = struc.Handle;
            LayerName = struc.LayerName;
        }

        #endregion

    }

    /// <summary>
    /// Internal Structure for entity creation
    /// </summary>
    public class EntityStruct : IEntityStruct
    {
        /// <summary>
        /// The Entity Type
        /// </summary>
        public Type EntityType {get; set;}

        /// <summary>
        /// The Handle of the Entity
        /// </summary>
        public string Handle {get; set;}

        /// <summary>
        /// A List of Entities
        /// </summary>
        public List<IEntityReference> References {get; set;}

        /// <summary>
        /// The Name of the Layer that the Entity is on
        /// </summary>
        public string LayerName {get; set;}

        /// <summary>
        ///  The SoftPointers for this struct
        /// </summary>
        public List<string> SoftPointers {get; set;} = new List<string>();

        /// <summary>
        /// Set a property in the entity
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="value">The value of the property</param>
        public virtual void SetProperty(string name, object value)
        {
            switch(name)
            {
                case "SoftPointer":
                    SoftPointers.Add(value as string);
                break;

                default:
                    SetPropertyDefault(name, value);
                break;
            }
        }

        /// <summary>
        /// Default Function for setting properties
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="value">The value to set for the property</param>
        private void SetPropertyDefault(string name, object value)
        {
            var property = this.GetType().GetProperty(name);

            // Get the type of the property
            var type = property.PropertyType;

            // Converty the object to the type of the property
            var settingValue = Convert.ChangeType(value, type);

            // Set the property
            this.GetType().GetProperty(name).SetValue(this, settingValue);
        }
    }
}