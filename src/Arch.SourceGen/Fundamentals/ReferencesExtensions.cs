using System.Text;
using Arch.SourceGen;

namespace ArchSourceGenerator;

public static class ReferencesExtensions
{
    public static StringBuilder AppendReferences(this StringBuilder sb, int amount)
    {
        for (var index = 0; index < amount; index++)
            sb.AppendReference(index);

        return sb;
    }

    public static StringBuilder AppendReference(this StringBuilder sb, int amount)
    {

        var generics = new StringBuilder().GenericWithoutBrackets(amount);
        var parameters = new StringBuilder().GenericRefParams(amount);

        var refStructs = new StringBuilder();
        for (var index = 0; index <= amount; index++)
            refStructs.AppendLine($"public Ref<T{index}> t{index};");

        var references = new StringBuilder();
        for (var index = 0; index <= amount; index++)
            references.AppendLine($"public ref T{index} t{index};");

        var assignRefStructs = new StringBuilder();
        for (var index = 0; index <= amount; index++)
            assignRefStructs.AppendLine($"t{index} = new Ref<T{index}>(ref t{index}Component);");

        var assignRefs = new StringBuilder();
        for (var index = 0; index <= amount; index++)
            assignRefs.AppendLine($"t{index} = ref t{index}Component;");


        var template =
            $$"""
            [SkipLocalsInit]
            public ref struct References<{{generics}}>
            {

            #if NETSTANDARD2_1 || NET6_0
                {{refStructs}}
            #else
                {{references}}
            #endif

                [SkipLocalsInit]
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public References({{parameters}}){

            #if NETSTANDARD2_1 || NET6_0
                {{assignRefStructs}}
            #else
                {{assignRefs}}
            #endif

                }
            }
            """;

        return sb.AppendLine(template);
    }

    public static StringBuilder AppendEntityReferences(this StringBuilder sb, int amount)
    {
        for (var index = 0; index < amount; index++)
            sb.AppendEntityReference(index);

        return sb;
    }

    public static StringBuilder AppendEntityReference(this StringBuilder sb, int amount)
    {

        var generics = new StringBuilder().GenericWithoutBrackets(amount);
        var parameters = new StringBuilder().GenericRefParams(amount);

        var refStructs = new StringBuilder();
        for (var index = 0; index <= amount; index++)
            refStructs.AppendLine($"public Ref<T{index}> t{index};");

        var references = new StringBuilder();
        for (var index = 0; index <= amount; index++)
            references.AppendLine($"public ref T{index} t{index};");

        var assignRefStructs = new StringBuilder();
        for (var index = 0; index <= amount; index++)
            assignRefStructs.AppendLine($"t{index} = new Ref<T{index}>(ref t{index}Component);");

        var assignRefs = new StringBuilder();
        for (var index = 0; index <= amount; index++)
            assignRefs.AppendLine($"t{index} = ref t{index}Component;");


        var template =
            $$"""
            [SkipLocalsInit]
            public ref struct EntityReferences<{{generics}}>
            {

            #if NETSTANDARD2_1 || NET6_0
                public ReadOnlyRef<Entity> Entity;
                {{refStructs}}
            #else
                public ref readonly Entity Entity;
                {{references}}
            #endif

                [SkipLocalsInit]
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public EntityReferences(in Entity entity, {{parameters}}){

            #if NETSTANDARD2_1 || NET6_0
                Entity = new ReadOnlyRef<Entity>(in entity);
                {{assignRefStructs}}
            #else
                Entity = ref entity;
                {{assignRefs}}
            #endif

                }
            }
            """;

        return sb.AppendLine(template);
    }
}
