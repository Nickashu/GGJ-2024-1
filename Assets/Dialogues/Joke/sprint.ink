INCLUDE ../Variables.ink

VAR random = 1
~random = RANDOM(1, 2)

{language == 1: -> main | -> main2}

=== main ===
{random:
- 1: Se eu fosse você, tentaria correr segurando "Shift" para dar esse pulo. Na verdade, esquece. Acho que não deu tempo de implementar...
- 2: O fato de você não conseguir correr pode ser encarado como falta de competência dos desenvolvedores ou como feature #vivendoeaprendendo
}
-> END

=== main2 ===
{random:
- 1: If I were you, I'd try running while holding 'Shift' to make that jump. Actually, forget it. I think they didn't have time to implement it...
- 2: The fact that you can't run can be seen as a lack of competence from the developers or as a feature #livingandlearning
}
-> END