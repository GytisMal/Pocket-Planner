CREATE TABLE categories (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    name VARCHAR (200) NOT NULL,
    pattern VARCHAR (800) NOT NULL UNIQUE,
);

INSERT INTO categories (name, pattern)
VALUES 
('Food', '\b(maxima|lidl|rimi|silas|iki|kebabai|kubas|norfa|norvilo|street|elija|valgykla|todze|hesburger|express|keturi simtai|aces|baras|kavine|alkava|jack pub|wolt|cafe|voster|laurus|studio|sushi|BOTTLERY|Talutti|COFFEE|OBUOLIO|Miltuoti|MENY|EXTRA|chicken|peppes|esso as|fruktmarked|YX|europris|burger|esso|coop|market|maxi|prix|kiwi|pizza|coop|7ELEVEN|ALLE|pietus)\b
'),
('Car', '\b(Neste|Stova|Circle K|jozita|Orlen|msi|baltic petroleum|bonsa|heksanas|daivanta|autoaibe|degaline|viada|plovykla|adcparking|regitra|DEGALINEJOZITA_BALTU|Mirigitos|CV|Aon|mitrochinienė)\b'),
('Clothes', '\b(New Yorker|Zara|Peek & Cloppenburg|Sportland|H&M|4F|A&G|Adidas|Nike|Apranga|Aprangos Galerija|Audimas|Camel|Active|Crocs|Cropp|Deichmann|Denim Dream|Guess|House|Jack&Jones|zalgirisshop|salamander|notino.lt|wawa|go9|HOUSE)\b'),
('House Hold Goods', '\b(Senukai|Ermitazas|kesko|Electrolux|Jysk|Ikea|Araneta|depo|moki vezi|simplea)\b'),
('Beauty', '\b(sultan|barbers|Fade|eurokos|drogas|douglas|cutters)\b'),
('Health', '\b(camelia|gintarine|vaistine|eurovaistine|benu|Norfos|apotheca|paneriu|pilies|klinika|dantu|Artdenta|viadenta|sumani)\b'),
('Loan', '\b(paskola)\b'),
('Rent', '\b(nuoma|ebendrabutis)\b'),
('Taxes', '\b(ignitis|dunokai|eso|energija|elektra|elektrum|enefit|elektrine|energy|vanduo|seb)\b'),
('Phone Taxes', '\b(bite|labas|tele2|telia|ezys|pildyk|bitė)\b'),
('Fun', '\b(cinema|tiketa|amusement park|event|hotel|cinemas|sporto|zalgirio|Tado|ALKSNYNES|SMILTYNES|M.K.CIURLIONIO|apverstas|dejavu|Interversus|Rumsiskes|GMANTIKA|FARMLAND|Ryanair|bowl )\b'),
('Public Transport', '\b(kautra|ziogas|transportas)\b'),
('Savings', '\b(taupyklę|taupymo)\b'),
('Salary', '\b(hiper|kariuomenė|transline|nordren|Daujotiene)\b'),
('Gift', '\b(geles|foto|geliu|MICKEVICIUTE|KOVTOROVOS|GĖLĖS)\b'),
('Education', '\b(ketbilietai|udemy|knygynas|norvegu24|audioteka)\b'),
('Sport', '\b(uab G sportas|SPAR|BODYPOWER|sportui)\b'),
('Additional Money Income', '\b(Ministerijos|perlaida)\b');