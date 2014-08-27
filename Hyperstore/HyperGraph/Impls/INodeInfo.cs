﻿using System;
namespace Hyperstore.Modeling.HyperGraph
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  Interface for node information.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public interface INodeInfo
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets the id.
        /// </summary>
        /// <value>
        ///  The identifier.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        Identity Id { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets the meta class id.
        /// </summary>
        /// <value>
        ///  The identifier of the schema.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        Identity SchemaId { get; }
    }

    public interface IRelationshipInfo : INodeInfo
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets the start.
        /// </summary>
        /// <value>
        ///  The identifier of the start.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        Identity StartId { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets the start meta class id.
        /// </summary>
        /// <value>
        ///  The identifier of the start schema.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        Identity StartSchemaId { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets the end.
        /// </summary>
        /// <value>
        ///  The identifier of the end.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        Identity EndId { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets the end meta class id.
        /// </summary>
        /// <value>
        ///  The identifier of the end schema.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        Identity EndSchemaId { get; }
    }
}
