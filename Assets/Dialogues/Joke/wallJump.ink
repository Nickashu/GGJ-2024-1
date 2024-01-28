INCLUDE ../Variables.ink

VAR random = 1
~random = RANDOM(1, 2)

{language == 1: -> main | -> main2}

=== main ===
{random:
- 1: Bom, acho que você sabe o que fazer com essas duas paredes próximas uma da outra, certo?
- 2: Não é possível que você esteja preso nessa parte, eu me recuso a acreditar
}
-> END

=== main2 ===
{random:
- 1: Well, I guess you know what to do with those two walls close to each other, right?
- 2: You're not stuck at this part, right? I refuse to believe it.
}
-> END