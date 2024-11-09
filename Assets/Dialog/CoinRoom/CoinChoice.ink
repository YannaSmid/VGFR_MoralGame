-> main

=== main ===
Oh no! Someone just stole the last money I had...
Now my grandma and I won't have anything to eat.
Do you have anything to spare?
    + [Yes]
        -> give_money
    + [No]
        -> refuse_money

=== give_money ===
You decided to give them your money.
-> DONE

=== refuse_money ===
You refused to give them your money.
-> DONE

=== DONE ===
-> END
