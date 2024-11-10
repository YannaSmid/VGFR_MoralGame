-> main

=== main ===
Hhmph...
You like my boots, eh?
No one can stop me with these shoes on. 
I can even reach to the top of that wall.
These shoes are my life.

    + [Complement shoes]
        -> complimented("complemented the guy about his shoes.")
    + [Steal the shoes]
        -> stolen("stole the shoes. You can jump higher now. But seems like you upset this guy...")
        
=== complimented(choice) ===
<i>You {choice}</i>
Ey, they are boots. Are you dumb?

-> END

=== stolen(choice) ===
<i>You {choice}</i>
Ey, you filthy thief. You will pay for this!
-> END
