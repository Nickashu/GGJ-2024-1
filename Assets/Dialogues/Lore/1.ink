INCLUDE ../Variables.ink

VAR random = 1
~random = RANDOM(1, 1)

{language == 1: -> main | -> main2}

=== main ===
{random:
- 1: Ok, você conseguiu chegar até aqui. Como prometido, vou explicar o que está acontecendo. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc vel gravida nisi. Nam eu lorem varius, imperdiet augue a, pulvinar augue. Aenean aliquet elit ut fringilla aliquam. Suspendisse vehicula justo vitae consectetur tincidunt. Nulla sollicitudin est eget suscipit volutpat.
}
-> END

=== main2 ===
{random:
- 1: Okay, you made it this far. As promised, I'll explain what's going on. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc vel gravida nisi. Nam eu lorem varius, imperdiet augue a, pulvinar augue. Aenean aliquet elit ut fringilla aliquam. Suspendisse vehicula justo vitae consectetur tincidunt. Nulla sollicitudin est eget suscipit volutpat.
}
-> END