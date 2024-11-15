-> main

=== main ===
Which cage will you open?
    + [Free the human]
        -> chosen("to help the human")
    + [Get the sword]
        -> chosen("to get the sword...")
        
=== chosen(cage) ===
<i>You chose {cage}</i>!
-> END

