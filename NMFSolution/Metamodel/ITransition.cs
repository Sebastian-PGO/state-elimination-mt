//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Models.Collections;
using NMF.Models.Expressions;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace TTC2017.StateElimination.Transitiongraph
{
    
    
    /// <summary>
    /// The public interface for Transition
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Transition))]
    [XmlDefaultImplementationTypeAttribute(typeof(Transition))]
    public interface ITransition : IModelElement
    {
        
        /// <summary>
        /// The label property
        /// </summary>
        string Label
        {
            get;
            set;
        }
        
        /// <summary>
        /// The probability property
        /// </summary>
        double Probability
        {
            get;
            set;
        }
        
        /// <summary>
        /// The source property
        /// </summary>
        IState Source
        {
            get;
            set;
        }
        
        /// <summary>
        /// The target property
        /// </summary>
        IState Target
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets fired before the Label property changes its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> LabelChanging;
        
        /// <summary>
        /// Gets fired when the Label property changed its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> LabelChanged;
        
        /// <summary>
        /// Gets fired before the Probability property changes its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> ProbabilityChanging;
        
        /// <summary>
        /// Gets fired when the Probability property changed its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> ProbabilityChanged;
        
        /// <summary>
        /// Gets fired before the Source property changes its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> SourceChanging;
        
        /// <summary>
        /// Gets fired when the Source property changed its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> SourceChanged;
        
        /// <summary>
        /// Gets fired before the Target property changes its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> TargetChanging;
        
        /// <summary>
        /// Gets fired when the Target property changed its value
        /// </summary>
        event System.EventHandler<ValueChangedEventArgs> TargetChanged;
    }
}

