INCLUDE ../Variables.ink

VAR random = 1
~random = RANDOM(1, 1)

{language == 1: -> main | -> main2}

=== main ===
{random:
- 1: Ok, eu te subestimei. Você até que tem habilidade
}
-> END

=== main2 ===
{random:
- 1: Okay, I underestimated you. You actually have some skills
}
-> END