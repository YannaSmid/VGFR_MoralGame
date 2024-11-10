-> main

=== main ===
Oh no! Someone just stole the last money I had.
I'm so hungry...
Do you have anything to spare?
    + [Yes]
        -> give_money
    + [No]
        -> refuse_money

=== give_money ===
Thank you so much for your generosity.
Now my little sister and I can finally eat something.
-> DONE

=== refuse_money ===
Oh..
I understand..
My little sister and I will figure out how to get food another way.
-> DONE

=== DONE ===
-> END
