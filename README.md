# FineGrain
Finite group study, abelians or not, quotient group, direct and semidirect product and many more...

Starting to study Quotient Group in Z.
```
var zn = new Group256(20);
var G = zn.Generate("Z", 1);
var H = zn.Generate("5Z", 5);
G.DisplayHead();
H.DisplayHead();

var Qg = new QuotientGroup<byte, Integer256>(G, H);
Qg.Details();
Qg.DisplayClasses();
```

Will output

```
|Z| = 20
IsGroup      : True
IsCommutative: True

|5Z| = 4
IsGroup      : True
IsCommutative: True

|Z/5Z| = 5 with |Z| = 20 and |5Z| = 4, OpBoth
IsGroup      : True
IsCommutative: True

@ = (  0)[  1]
a = (  1)[ 20]
b = (  2)[ 10]
c = (  3)[ 20]
d = (  4)[  5]

|Z/5Z| = 5 with |Z| = 20 and |5Z| = 4, OpBoth
 *|@ a b c d
--|----------
 @|@ a b c d
 a|a b c d @
 b|b c d @ a
 c|c d @ a b
 d|d @ a b c


Class of : (  0)[  1]
    Represent
    (  0)[  1]
    (  5)[  4]
    ( 10)[  2]
    ( 15)[  4]
Class of : (  1)[ 20]
    Represent
    (  1)[ 20]
    (  6)[ 10]
    ( 11)[ 20]
    ( 16)[  5]
Class of : (  2)[ 10]
    Represent
    (  2)[ 10]
    (  7)[ 20]
    ( 12)[  5]
    ( 17)[ 20]
Class of : (  3)[ 20]
    Represent
    (  3)[ 20]
    (  8)[  5]
    ( 13)[ 20]
    ( 18)[ 10]
Class of : (  4)[  5]
    Represent
    (  4)[  5]
    (  9)[ 20]
    ( 14)[ 10]
    ( 19)[ 20]

```

### Simple S3, the group of permutations

```
var sn = new Sigma(3);
var H0 = sn.Generate("H0", (1, 2, 3), (1, 2));
var H1 = sn.Generate("H1", (1, 2, 3));
H0.DisplayElements();
H1.DisplayElements();

var Qg0 = new QuotientGroup<byte, Permutation>(H0, H1);
Qg0.Details();
Qg0.DisplayClasses();
```

Will output

```
|H0| = 6
IsGroup      : True
IsCommutative:False

@ = ( 1  2  3)[ 1+]
a = ( 1  3  2)[ 2-]
b = ( 2  1  3)[ 2-]
c = ( 3  2  1)[ 2-]
d = ( 2  3  1)[ 3+]
e = ( 3  1  2)[ 3+]

|H1| = 3
IsGroup      : True
IsCommutative: True

@ = ( 1  2  3)[ 1+]
a = ( 2  3  1)[ 3+]
b = ( 3  1  2)[ 3+]

|H0/H1| = 2 with |H0| = 6 and |H1| = 3, OpBoth
IsGroup      : True
IsCommutative: True

@ = ( 1  2  3)[ 1+]
a = ( 1  3  2)[ 2-]

|H0/H1| = 2 with |H0| = 6 and |H1| = 3, OpBoth
 *|@ a
--|----
 @|@ a
 a|a @


Class of : ( 1  2  3)[ 1+]
    Represent
    ( 1  2  3)[ 1+]
    ( 2  3  1)[ 3+]
    ( 3  1  2)[ 3+]
Class of : ( 1  3  2)[ 2-]
    Represent
    ( 1  3  2)[ 2-]
    ( 2  1  3)[ 2-]
    ( 3  2  1)[ 2-]


```

Then with Sigma6
```
|H0| = 6
IsGroup      : True
IsCommutative: True

@ = ( 1  2  3  4  5  6)[ 1+]
a = ( 1  2  3  5  4  6)[ 2-]
b = ( 2  3  1  4  5  6)[ 3+]
c = ( 3  1  2  4  5  6)[ 3+]
d = ( 2  3  1  5  4  6)[ 6-]
e = ( 3  1  2  5  4  6)[ 6-]

|H1| = 2
IsGroup      : True
IsCommutative: True

@ = ( 1  2  3  4  5  6)[ 1+]
a = ( 1  2  3  5  4  6)[ 2-]

|H0/H1| = 3 with |H0| = 6 and |H1| = 2, OpBoth
IsGroup      : True
IsCommutative: True

@ = ( 1  2  3  4  5  6)[ 1+]
a = ( 2  3  1  4  5  6)[ 3+]
b = ( 3  1  2  4  5  6)[ 3+]

|H0/H1| = 3 with |H0| = 6 and |H1| = 2, OpBoth
 *|@ a b
--|------
 @|@ a b
 a|a b @
 b|b @ a


Class of : ( 1  2  3  4  5  6)[ 1+]
    Represent
    ( 1  2  3  4  5  6)[ 1+]
    ( 1  2  3  5  4  6)[ 2-]
Class of : ( 2  3  1  4  5  6)[ 3+]
    Represent
    ( 2  3  1  4  5  6)[ 3+]
    ( 2  3  1  5  4  6)[ 6-]
Class of : ( 3  1  2  4  5  6)[ 3+]
    Represent
    ( 3  1  2  4  5  6)[ 3+]
    ( 3  1  2  5  4  6)[ 6-]

```
