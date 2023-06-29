// <auto-generated />
#pragma warning disable 1570, 1591

using System;
using Microsoft.ML.Probabilistic;
using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Collections;
using Microsoft.ML.Probabilistic.Factors;

namespace Models
{
	/// <summary>
	/// Generated algorithm for performing inference.
	/// </summary>
	/// <remarks>
	/// If you wish to use this class directly, you must perform the following steps:
	/// 1) Create an instance of the class.
	/// 2) Set the value of any externally-set fields e.g. data, priors.
	/// 3) Call the Execute(numberOfIterations) method.
	/// 4) Use the XXXMarginal() methods to retrieve posterior marginals for different variables.
	/// 
	/// Generated by Infer.NET 0.4.2301.301 at 8:53 PM on Saturday, June 24, 2023.
	/// </remarks>
	public partial class Model23_EP : IGeneratedAlgorithm
	{
		#region Fields
		/// <summary>True if Constant has executed. Set this to false to force re-execution of Constant</summary>
		public bool Constant_isDone;
		/// <summary>Field backing the NumberOfIterationsDone property</summary>
		private int numberOfIterationsDone;
		/// <summary>Message to marginal of 'successCount'</summary>
		public Discrete successCount_marginal_F;
		/// <summary>Message to marginal of 'vbool48'</summary>
		public Bernoulli vbool48_marginal_F;
		/// <summary>Message to marginal of 'vbool49'</summary>
		public Bernoulli vbool49_marginal_F;
		#endregion

		#region Properties
		/// <summary>The number of iterations done from the initial state</summary>
		public int NumberOfIterationsDone
		{
			get {
				return this.numberOfIterationsDone;
			}
		}

		#endregion

		#region Methods
		/// <summary>Computations that do not depend on observed values</summary>
		private void Constant()
		{
			if (this.Constant_isDone) {
				return ;
			}
			Discrete successCount_F = ArrayHelper.MakeUniform<Discrete>(Discrete.Uniform(163));
			this.successCount_marginal_F = ArrayHelper.MakeUniform<Discrete>(Discrete.Uniform(163));
			Discrete successCount_use_B = ArrayHelper.MakeUniform<Discrete>(Discrete.Uniform(163));
			// Message to 'successCount' from Binomial factor
			successCount_F = BinomialOp.SampleAverageConditional(0.01, 162, successCount_F);
			// Message to 'successCount_marginal' from Variable factor
			this.successCount_marginal_F = VariableOp.MarginalAverageConditional<Discrete>(successCount_use_B, successCount_F, this.successCount_marginal_F);
			Bernoulli vbool48_F = default(Bernoulli);
			this.vbool48_marginal_F = Bernoulli.Uniform();
			Bernoulli vbool48_use_B = Bernoulli.Uniform();
			// Message to 'vbool48' from IsGreaterThan factor
			vbool48_F = IsGreaterThanOp.IsGreaterThanAverageConditional(1, successCount_F);
			// Message to 'vbool48_marginal' from DerivedVariable factor
			this.vbool48_marginal_F = DerivedVariableOp.MarginalAverageConditional<Bernoulli>(vbool48_use_B, vbool48_F, this.vbool48_marginal_F);
			Bernoulli vbool49_F = default(Bernoulli);
			this.vbool49_marginal_F = Bernoulli.Uniform();
			Bernoulli vbool49_use_B = Bernoulli.Uniform();
			// Message to 'vbool49' from Not factor
			vbool49_F = BooleanNotOp.NotAverageConditional(vbool48_F);
			// Message to 'vbool49_marginal' from DerivedVariable factor
			this.vbool49_marginal_F = DerivedVariableOp.MarginalAverageConditional<Bernoulli>(vbool49_use_B, vbool49_F, this.vbool49_marginal_F);
			this.Constant_isDone = true;
		}

		/// <summary>Update all marginals, by iterating message passing the given number of times</summary>
		/// <param name="numberOfIterations">The number of times to iterate each loop</param>
		/// <param name="initialise">If true, messages that initialise loops are reset when observed values change</param>
		private void Execute(int numberOfIterations, bool initialise)
		{
			this.Constant();
			this.numberOfIterationsDone = numberOfIterations;
		}

		/// <summary>Update all marginals, by iterating message-passing the given number of times</summary>
		/// <param name="numberOfIterations">The total number of iterations that should be executed for the current set of observed values.  If this is more than the number already done, only the extra iterations are done.  If this is less than the number already done, message-passing is restarted from the beginning.  Changing the observed values resets the iteration count to 0.</param>
		public void Execute(int numberOfIterations)
		{
			this.Execute(numberOfIterations, true);
		}

		/// <summary>Get the observed value of the specified variable.</summary>
		/// <param name="variableName">Variable name</param>
		public object GetObservedValue(string variableName)
		{
			throw new ArgumentException("Not an observed variable name: "+variableName);
		}

		/// <summary>Get the marginal distribution (computed up to this point) of a variable</summary>
		/// <param name="variableName">Name of the variable in the generated code</param>
		/// <returns>The marginal distribution computed up to this point</returns>
		/// <remarks>Execute, Update, or Reset must be called first to set the value of the marginal.</remarks>
		public object Marginal(string variableName)
		{
			if (variableName=="successCount") {
				return this.SuccessCountMarginal();
			}
			if (variableName=="vbool48") {
				return this.Vbool48Marginal();
			}
			if (variableName=="vbool49") {
				return this.Vbool49Marginal();
			}
			throw new ArgumentException("This class was not built to infer "+variableName);
		}

		/// <summary>Get the marginal distribution (computed up to this point) of a variable, converted to type T</summary>
		/// <typeparam name="T">The distribution type.</typeparam>
		/// <param name="variableName">Name of the variable in the generated code</param>
		/// <returns>The marginal distribution computed up to this point</returns>
		/// <remarks>Execute, Update, or Reset must be called first to set the value of the marginal.</remarks>
		public T Marginal<T>(string variableName)
		{
			return Distribution.ChangeType<T>(this.Marginal(variableName));
		}

		/// <summary>Get the query-specific marginal distribution of a variable.</summary>
		/// <param name="variableName">Name of the variable in the generated code</param>
		/// <param name="query">QueryType name. For example, GibbsSampling answers 'Marginal', 'Samples', and 'Conditionals' queries</param>
		/// <remarks>Execute, Update, or Reset must be called first to set the value of the marginal.</remarks>
		public object Marginal(string variableName, string query)
		{
			if (query=="Marginal") {
				return this.Marginal(variableName);
			}
			throw new ArgumentException(((("This class was not built to infer \'"+variableName)+"\' with query \'")+query)+"\'");
		}

		/// <summary>Get the query-specific marginal distribution of a variable, converted to type T</summary>
		/// <typeparam name="T">The distribution type.</typeparam>
		/// <param name="variableName">Name of the variable in the generated code</param>
		/// <param name="query">QueryType name. For example, GibbsSampling answers 'Marginal', 'Samples', and 'Conditionals' queries</param>
		/// <remarks>Execute, Update, or Reset must be called first to set the value of the marginal.</remarks>
		public T Marginal<T>(string variableName, string query)
		{
			return Distribution.ChangeType<T>(this.Marginal(variableName, query));
		}

		private void OnProgressChanged(ProgressChangedEventArgs e)
		{
			// Make a temporary copy of the event to avoid a race condition
			// if the last subscriber unsubscribes immediately after the null check and before the event is raised.
			EventHandler<ProgressChangedEventArgs> handler = this.ProgressChanged;
			if (handler!=null) {
				handler(this, e);
			}
		}

		/// <summary>Reset all messages to their initial values.  Sets NumberOfIterationsDone to 0.</summary>
		public void Reset()
		{
			this.Execute(0);
		}

		/// <summary>Set the observed value of the specified variable.</summary>
		/// <param name="variableName">Variable name</param>
		/// <param name="value">Observed value</param>
		public void SetObservedValue(string variableName, object value)
		{
			throw new ArgumentException("Not an observed variable name: "+variableName);
		}

		/// <summary>
		/// Returns the marginal distribution for 'successCount' given by the current state of the
		/// message passing algorithm.
		/// </summary>
		/// <returns>The marginal distribution</returns>
		public Discrete SuccessCountMarginal()
		{
			return this.successCount_marginal_F;
		}

		/// <summary>Update all marginals, by iterating message-passing an additional number of times</summary>
		/// <param name="additionalIterations">The number of iterations that should be executed, starting from the current message state.  Messages are not reset, even if observed values have changed.</param>
		public void Update(int additionalIterations)
		{
			this.Execute(checked(this.numberOfIterationsDone+additionalIterations), false);
		}

		/// <summary>
		/// Returns the marginal distribution for 'vbool48' given by the current state of the
		/// message passing algorithm.
		/// </summary>
		/// <returns>The marginal distribution</returns>
		public Bernoulli Vbool48Marginal()
		{
			return this.vbool48_marginal_F;
		}

		/// <summary>
		/// Returns the marginal distribution for 'vbool49' given by the current state of the
		/// message passing algorithm.
		/// </summary>
		/// <returns>The marginal distribution</returns>
		public Bernoulli Vbool49Marginal()
		{
			return this.vbool49_marginal_F;
		}

		#endregion

		#region Events
		/// <summary>Event that is fired when the progress of inference changes, typically at the end of one iteration of the inference algorithm.</summary>
		public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
		#endregion

	}

}
