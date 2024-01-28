INCLUDE ../Variables.ink

VAR random = 1
~random = RANDOM(1, 2)

{language == 1: -> main | -> main2}

=== main ===
{random:
- 1: Tá precisando treinar mais um pouquinho amigo... Te falta ódio
- 2: Ah que isso, não é como se os controles estivessem extremamente bugados. Eu acredito em você!
}
-> END

=== main2 ===
{random:
- 1: You need to train a bit more, buddy... You lack hatred
- 2: Oh, come on, it's not like the controls are extremely glitchy. I believe in you!
}
-> END