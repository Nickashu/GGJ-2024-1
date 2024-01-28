INCLUDE ../Variables.ink

VAR random = 1
~random = RANDOM(1, 1)

{language == 1: -> main | -> main2}

=== main ===
{random:
- 1: Esta droga aqui na frente é uma criação minha. Ela vai deixar você nas alturas....
}
-> END

=== main2 ===
{random:
- 1: This drug right here is my creation. You'll definetely get high my friend...
}
-> END