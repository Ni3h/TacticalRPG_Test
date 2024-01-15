# Class Structure

## Decription

A `class` is a language construct in C# that contains zero or more fields, properties, and/or methods. Each of these concepts are briefly desribed below:

### Fields
A field is a variable that lies outside the scope of a method. Most often, these are tied to the instance of the class they're within, but can be static as well.
```cs
// Syntax
public class Foo {
    int someField;
}
// Usage
var foo = new Foo();
foo.someField;
```

### Properties
A property is a special type of language construct that allows for a getter/setter method to be written in a way that is transparent to the user of a class. As with fields, they can be static or instanced. Unless you're using an auto-property, the keywords `get` and/or `set` will be their own blocks containing one or more statements.
```cs
// Syntax
public class Foo {
    int SomeProperty {
        // Auto-implemented getter.
        get;
        // Manually written setter.
        set {
            backingField = value;
        }
    }
    int backingField;
}
// Usage
var foo = new Foo();
foo.SomeProperty;
```

### Methods
A method is a scoped code block within a class that contains zero or more statements. Another term used frequently for methods is "function", the operative difference being that methods are code blocks that are part of a class, and functions are code blocks independent of any class. As C# is an object-oriented language, writing functions is impossible, so the two terms can be used interchangeably.
```cs
// Syntax
public class Foo {
    int SomeMethod() {
        return 69;
    }
}
// Usage
var foo = new Foo();
foo.SomeMethod();
```

## Convention

### Fields
A field should generally be used to store information used within the properties/methods of a class. If you require a variable to be accesible from outside a class, write a property that uses an internal field instead:

```cs
public class Foo {
    public int Health {
        get {
            return health;
        }
        set {
            health = (value > MAX_HP) ? MAX_HP : value;
        }
    }
    private int health;
}
```

A private field should always be written in camelCase, and the name should match the property using it as a backing field, if one exists.

#### Rationale
Code evolves over time, especially in a large project, and changing the API of a class means that any code using it needs to be refactored. Often times, a field being set or accessed will require some side-effect to occur (e.g. a health field validating that the assigned value won't put it above the maximum allowed health).

As values such as this are often called from outside a class, exposing them as a property instead will ensure that the user doesn't need to change their code when a new side-effect needs to be added.

#### Caveats
One Unity-specific exception to these rules is with fields that are used as part of your Script components. Only fields can be explosed to the Unity editor, thus the following usage is suggested instead:
```cs
public class Foo : BaseBehavior {
    [SerializeField]
    private int Health;
}
```
The [`SerializeField`](https://docs.unity3d.com/ScriptReference/SerializeField.html) attribute allows non-public fields to be exposed to the editor as if they were public. This improves readability to any users of this class while still allowing it to take full advantage of Unity's functionality.

These special fields should be written in PascalCase, to convey to other developers that they should not be be used as an ordinary field from other places within the class.

### Properties
Property names should be written in PascalCase. If a property does not currently require any logic to take place, it can be written as such:
```cs
public class Foo {
    public CurrentHealth {
        // Auto-implemented accessors can be placed on the same line.
        get; private set;
    } = 10; // An initial value can be set for auto-properties.
}
```
Access to the property should be set as strictly as possible; generally, this means that the getter can be public, but the setter will be private.

If logic is required for a property, a field can be used to store a value for this property, but the field should be private and should use the same name as a property, written as camelCase.

#### Rationale
Properties are a powerful concept in C#, making standard getter/setter methods unnecessary while maintaining their flexibility.

Due to their ease of use, fields don't need to be exposed outside of a class for reasons outlined above, and properties should be used exclusively for this purpose.

As properties allow their getter/setter methods to have different access levels, care should be taken that a setter isn't accessible publicly unless necessary. This is a foundation of functional programming, preventing internal state from being mutated externally as much as possible.

### Methods
A method should contain no more than 10-20 lines of logic. A method exceeding this can likely have logical sections broken off into a smaller function.

Methods should be named with PascalCase, and parameters should be named with camelCase. If the method is public, it should be documented what each parameter is for.
```cs
/// <summary>
/// Renders a health bar with the provided health value.
/// The rendered bar will be placed over the provided Unit.
/// </summary>
/// <param name="health">The current health to display.</param>
/// <param name="character">The character to render this health bar for.</param>
public void RenderHealthBar(int health, Unit character) {
    // internal logic
}
```

#### Rationale
Methods are where most of your code will be written, and as such, care needs to be taken not to let them get unwieldy. Many programs exist that contain 100+ line methods, and growing to this size [causes several issues](https://softwareengineering.stackexchange.com/questions/133404/what-is-the-ideal-length-of-a-method-for-you) with modifying them, as the local state can become too complex for a developer to be mindful of when making changes.

Breaking a large method into smaller methods that are then composed together allows for the developer to more easily understand what each separate call is doing, and discern the overall functionality of a method based on this. It also compartmentalizes pieces of local state that may be needed for one section, but not another.

For documentation, private methods can often slide by with a few basic comments, or none at all for trivial operations. Public methods should contain XML doc comments, so that users of this function can at a glance know what it will do. [Here](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/) is a short guide on how they're written. Using a format other than XML doc is recommended against, as [IntelliSense](https://learn.microsoft.com/en-us/visualstudio/ide/visual-csharp-intellisense?view=vs-2022) is unable to interpret arbitrary comments. 