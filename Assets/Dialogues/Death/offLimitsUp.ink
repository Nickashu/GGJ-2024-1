INCLUDE ../Variables.ink

VAR random = 1

{language == 1: -> main | -> main2}

=== main ===
~random = RANDOM(1, 2)
{random:
- 1: Eita! Tá tentando voar amigão??
- 2: Bom.... Acho que essa droga tá um pouco mais forte do que eu imaginava
}
-> END

=== main2 ===
~random = RANDOM(1, 2)
{random:
- 1: Whoa! Are you trying to fly, buddy??
- 2: Well... I think this stuff is a bit stronger than I imagined.
}
-> END