INCLUDE ../Variables.ink

VAR random = 1
~random = RANDOM(1, 1)

{language == 1: -> main | -> main2}

=== main ===
{random:
- 1: Opa opa opa! Você tá tentando sair?? Me desculpe mas eu não posso permitir isto ainda
}
-> END

=== main2 ===
{random:
- 1: Whoa whoa whoa! Are you trying to leave?? I'm sorry, but I can't allow that yet
}
-> END