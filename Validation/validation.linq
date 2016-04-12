<Query Kind="Program" />

void Main()
{
	TestClass[] testClasses =
		{
			new TestClass()
			{
				Name = "Ken",
				Email = "Longlonglonglongemaillllll@looongestemail.com"
			},
			new TestClass()
			{
				Name = "LongLongLongLongNamedPerson",
				DateOfBirth = DateTime.Now
			}
		};
		
	TestClassValidator validator = new TestClassValidator();
	var errors = validator.Validate(testClasses);
	errors.Dump();
}

public class ValidationError
{
	public ValidationError(string message)
	{
		this.Message = message;
	}
	
	public string Message { get; set; }
}

public interface IValidator
{
	bool CanValidate(object subject);
	IEnumerable<ValidationError> Validate(object subject);
}

public interface IValidator<T> : IValidator
{
	bool CanValidate(T subject);
	IEnumerable<ValidationError> Validate(T subject);
}

public abstract class Validator<T> : IValidator<T>
{
	public Validator(string name)
	{
		this.Name = name;
	}

	public string Name { get; set; }

	public virtual bool CanValidate(T subject)
	{
		return true;
	}

	public abstract IEnumerable<ValidationError> Validate(T subject);

	protected ValidationError CreateError(string message)
	{
		return new ValidationError(string.Format("{0}: {1}", this.Name, message));
	}

	bool IValidator.CanValidate(object subject)
	{
		return this.CanValidate((T)subject);
	}

	IEnumerable<ValidationError> IValidator.Validate(object subject)
    {
		return this.Validate((T)subject);
	}
}

public class RequiredValidator<T> : Validator<T>
{
	public RequiredValidator(string name)
		: base(name)
	{
		
	}

	public override IEnumerable<ValidationError> Validate(T subject)
	{
		if(EqualityComparer<T>.Default.Equals(subject, default(T)))
			yield return this.CreateError("Is required");
	}
}

public class StringLengthValidator: Validator<string>
{
	private readonly int maxLength;

	public StringLengthValidator(string name, int maxLength)
		: base(name)
	{
		this.maxLength = maxLength;
	}

	public override bool CanValidate(string subject)
	{
		return subject != null;
	}

	public override IEnumerable<ValidationError> Validate(string subject)
	{
		if(subject.Length > maxLength)
			yield return this.CreateError(string.Format("Length greater than {0}", maxLength));
	}
}

public class PropertyValidator<TClass, TProperty> : Validator<TClass>
{
	private IValidator<TProperty>[] validators;
	private Func<TClass, TProperty> selector;

	public PropertyValidator(string name, Func<TClass, TProperty> selector, params IValidator<TProperty>[] validators)
		: base(name)
	{
		this.validators = validators;
		this.selector = selector;
	}

	public override IEnumerable<ValidationError> Validate(TClass subject)
	{
		var property = this.selector(subject);
	
		var errors =
			this.validators
				.Where(v => v.CanValidate(property))
				.SelectMany(v => v.Validate(property))
				.ToList();

		return errors;
    }
}

public class TestClassValidator
{
	private static readonly List<IValidator> validators =
		new List<IValidator>()
		{
			//All this name passing could be better done
			new PropertyValidator<TestClass, string>(
				"Name", 
				t => t.Name,
				new RequiredValidator<string>("Name"),
				new StringLengthValidator("Name", 10)
			),
			new PropertyValidator<TestClass, string>(
				"Email",
				t => t.Email,
				new RequiredValidator<string>("Email"),
				new StringLengthValidator("Email", 15)
			),
			new PropertyValidator<TestClass, DateTime>(
				"DateOfBirth",
				t => t.DateOfBirth,
				new RequiredValidator<DateTime>("DateOfBirth")
			)
		};

	public TestClassValidator()
	{
		
	}

	public IEnumerable<ValidationError> Validate(TestClass subject, string identifier)
	{
		var errors =
			validators
				.Where(v => v.CanValidate(subject))
				.SelectMany(v => v.Validate(subject))
				.ToList();

		return
			errors.Select(e =>
			{
				e.Message = string.Format("{0}.{1}", identifier, e.Message);
				return e;
			});
	}

	public IEnumerable<ValidationError> Validate(IEnumerable<TestClass> subjects)
	{
		return subjects
			.SelectMany((testClass, index) => 
				this.Validate(
					testClass, 
					string.Format("[{0}-{1}]", index, testClass.Name)));
	}
}

public class TestClass
{
	public string Name { get; set; }
	public string Email { get; set; }
	public DateTime DateOfBirth { get; set; }
}


