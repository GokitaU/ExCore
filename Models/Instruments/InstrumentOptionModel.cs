using System;
using Core.EnumSpace;
using FluentValidation;

namespace Core.ModelSpace
{
  /// <summary>
  /// Definition
  /// </summary>
  public interface IInstrumentOptionModel : IInstrumentModel
  {
    /// <summary>
    /// Open interest
    /// </summary>
    double? OpenInterest { get; set; }

    /// <summary>
    /// Strike price
    /// </summary>
    double? Strike { get; set; }

    /// <summary>
    /// The name of the underlying instrument
    /// </summary>
    string Symbol { get; set; }

    /// <summary>
    /// CALL or PUT
    /// </summary>
    OptionSideEnum? Side { get; set; }

    /// <summary>
    /// Expiration date
    /// </summary>
    DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Reference to the complex data point
    /// </summary>
    IInstrumentOptionGreekModel Greeks { get; set; }
  }

  /// <summary>
  /// Implementation
  /// </summary>
  public class InstrumentOptionModel : InstrumentModel, IInstrumentOptionModel
  {
    /// <summary>
    /// Open interest
    /// </summary>
    public virtual double? OpenInterest { get; set; }

    /// <summary>
    /// Strike price
    /// </summary>
    public virtual double? Strike { get; set; }

    /// <summary>
    /// The name of the underlying instrument
    /// </summary>
    public virtual string Symbol { get; set; }

    /// <summary>
    /// CALL or PUT
    /// </summary>
    public virtual OptionSideEnum? Side { get; set; }

    /// <summary>
    /// Expiration date
    /// </summary>
    public virtual DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Reference to the complex data point
    /// </summary>
    public virtual IInstrumentOptionGreekModel Greeks { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public InstrumentOptionModel()
    {
      Greeks = new InstrumentOptionGreekModel();
    }
  }

  /// <summary>
  /// Validation rules
  /// </summary>
  public class InstrumentOptionValidation : AbstractValidator<IInstrumentOptionModel>
  {
    public InstrumentOptionValidation()
    {
      RuleFor(o => o.Side).NotNull().WithMessage("No side");
      RuleFor(o => o.Strike).NotNull().NotEqual(0).WithMessage("No strike");
      RuleFor(o => o.ExpirationDate).NotNull().WithMessage("No expiration date");
    }
  }
}
