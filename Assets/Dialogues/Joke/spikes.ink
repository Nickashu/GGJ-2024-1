INCLUDE ../Variables.ink

VAR random = 1
~random = RANDOM(1, 1)

{language == 1: -> main | -> main2}

=== main ===
{random:
- 1: Ok... Agora eu quero ver você passar desses espinhos (ou morrer tentando)
}
-> END

=== main2 ===
{random:
- 1: Okay... Now I want to see you get past those spikes (or die trying)
}
-> END