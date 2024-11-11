-> main

=== main ===
Help! Help!
I need help!
I'm trapped by the fire.
    + [Help them]
            -> chosen("to help the villager. Go talk to him")
    + [Don't help them]
        -> chosen("to not help the villager.")
    
=== chosen(fire) ===
You chose {fire}.
-> END


