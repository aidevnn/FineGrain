# FineGrain
Finite group study, abelians or not, quotient group, direct and semidirect product and many more...

Starting to study Quotient Group in Z.
```
var z20 = new Group256(20);
z20.GenerateAll();
var G = new GroupSubSet<byte, Integer256>(z20, z20.Elements(), name: "Z");
var H = new Monogenic<byte, Integer256>(z20, z20.CreateElement(5), name: "5Z");
G.DisplayHead();
H.DisplayHead();

var Qg = new GroupQuotient<byte, Integer256>(G, H, GroupOpLR.Left);
Qg.Details();

foreach (var e in Qg.ClassOf)
    e.Value.Display();
```

Will output

```
|Z| = 20
IsGroup      : True
IsCommutative: True

|5Z| = 4
IsGroup      : True
IsCommutative: True

|Z/5Z| = 5 with |Z| = 20 and |5Z| = 4
IsGroup      : True
IsCommutative: True

@ = (  0)[  1]
a = (  1)[  5]
b = (  2)[  5]
c = (  3)[  5]
d = (  4)[  5]

|Z/5Z| = 5 with |Z| = 20 and |5Z| = 4
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

### More complex, S4, the group of permutations

```
var sn = new Sigma(4);
sn.GenerateAll();
var G = new GroupSubSet<byte, Permutation>(sn, sn.Elements(), "S4");
G.DisplayHead();

var H = new Monogenic<byte, Permutation>(sn, sn.kCycle(4), "H");
H.Details();

var Qg = new GroupQuotient<byte, Permutation>(G, H, GroupOpLR.Left);
Qg.Details();

foreach (var e in Qg.ClassOf)
    e.Value.Display();
```

Will output

```
|S4| = 24
IsGroup      : True
IsCommutative:False

|H| = 4
IsGroup      : True
IsCommutative: True

@ = ( 1  2  3  4)[ 1+]
a = ( 3  4  1  2)[ 2+]
b = ( 2  3  4  1)[ 4-]
c = ( 4  1  2  3)[ 4-]

|H| = 4
 *|@ a b c
--|--------
 @|@ a b c
 a|a @ c b
 b|b c a @
 c|c b @ a


|S4/H| = 6 with |S4| = 24 and |H| = 4
IsGroup      : True
IsCommutative:False

@ = ( 1  2  3  4)[ 1+]
a = ( 1  2  4  3)[ 2-]
b = ( 1  3  2  4)[ 2-]
c = ( 1  4  3  2)[ 2-]
d = ( 1  3  4  2)[ 3+]
e = ( 1  4  2  3)[ 3+]

|S4/H| = 6 with |S4| = 24 and |H| = 4
 *|@ a b c d e
--|------------
 @|@ a b c d e
 a|a @ d e b c
 b|b e @ d c a
 c|c d e @ a b
 d|d c a b e @
 e|e b c a @ d


Class of : ( 1  2  3  4)[ 1+]
    Represent
    ( 1  2  3  4)[ 1+]
    ( 2  3  4  1)[ 4-]
    ( 3  4  1  2)[ 2+]
    ( 4  1  2  3)[ 4-]
Class of : ( 1  2  4  3)[ 2-]
    Represent
    ( 1  2  4  3)[ 2-]
    ( 2  4  3  1)[ 3+]
    ( 3  1  2  4)[ 3+]
    ( 4  3  1  2)[ 4-]
Class of : ( 1  3  2  4)[ 2-]
    Represent
    ( 1  3  2  4)[ 2-]
    ( 2  4  1  3)[ 4-]
    ( 3  2  4  1)[ 3+]
    ( 4  1  3  2)[ 3+]
Class of : ( 1  3  4  2)[ 3+]
    Represent
    ( 1  3  4  2)[ 3+]
    ( 2  1  3  4)[ 2-]
    ( 3  4  2  1)[ 4-]
    ( 4  2  1  3)[ 3+]
Class of : ( 1  4  2  3)[ 3+]
    Represent
    ( 1  4  2  3)[ 3+]
    ( 2  3  1  4)[ 3+]
    ( 3  1  4  2)[ 4-]
    ( 4  2  3  1)[ 2-]
Class of : ( 1  4  3  2)[ 2-]
    Represent
    ( 1  4  3  2)[ 2-]
    ( 2  1  4  3)[ 2+]
    ( 3  2  1  4)[ 2-]
    ( 4  3  2  1)[ 2+]


```

### Normal Group coming soon