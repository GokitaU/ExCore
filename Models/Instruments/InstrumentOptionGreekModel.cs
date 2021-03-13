using System;
using Core.EnumSpace;
using FluentValidation;

namespace Core.ModelSpace
{
  /// <summary>
  /// Definition
  /// </summary>
  public interface IInstrumentOptionGreekModel : IBaseModel
  {
    /// <summary>
    /// Delta
    /// </summary>
    double? Delta { get; set; }

    /// <summary>
    /// Gamma
    /// </summary>
    double? Gamma { get; set; }

    /// <summary>
    /// Theta
    /// </summary>
    double? Theta { get; set; }

    /// <summary>
    /// Vega
    /// </summary>
    double? Vega { get; set; }

    /// <summary>
    /// Interest rate
    /// </summary>
    double? Interest { get; set; }

    /// <summary>
    /// Distribution density function
    /// </summary>
    double? Distribution { get; set; }

    /// <summary>
    /// Final IV
    /// </summary>
    double? Iv { get; set; }

    /// <summary>
    /// IV on the BID
    /// </summary>
    double? BidIv { get; set; }

    /// <summary>
    /// IV on the ASK
    /// </summary>
    double? AskIv { get; set; }
  }

  /// <summary>
  /// Implementation
  /// </summary>
  public class InstrumentOptionGreekModel : BaseModel, IInstrumentOptionGreekModel
  {
    /// <summary>
    /// Delta
    /// </summary>
    public virtual double? Delta { get; set; }

    /// <summary>
    /// Gamma
    /// </summary>
    public virtual double? Gamma { get; set; }

    /// <summary>
    /// Theta
    /// </summary>
    public virtual double? Theta { get; set; }

    /// <summary>
    /// Vega
    /// </summary>
    public virtual double? Vega { get; set; }

    /// <summary>
    /// Interest rate
    /// </summary>
    public virtual double? Interest { get; set; }

    /// <summary>
    /// Distribution density function
    /// </summary>
    public virtual double? Distribution { get; set; }

    /// <summary>
    /// Final IV
    /// </summary>
    public virtual double? Iv { get; set; }

    /// <summary>
    /// IV on the BID
    /// </summary>
    public virtual double? BidIv { get; set; }

    /// <summary>
    /// IV on the ASK
    /// </summary>
    public virtual double? AskIv { get; set; }
  }

  /// <summary>
  /// Validation rules
  /// </summary>
  public class InstrumentOptionGreekValidation : AbstractValidator<IInstrumentOptionGreekModel>
  {
    public InstrumentOptionGreekValidation()
    {
    }
  }
}
