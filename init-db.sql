-- Drop & recreate database
IF DB_ID('NorthwindDemo') IS NOT NULL
BEGIN
    ALTER DATABASE NorthwindDemo SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE NorthwindDemo;
END;
GO

CREATE DATABASE NorthwindDemo;
GO
USE NorthwindDemo;
GO

-- ==============================
-- Table: Categories
-- ==============================
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CategoryName NVARCHAR(15) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Picture VARBINARY(MAX) NULL,

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION
);
GO

-- ==============================
-- Seed data for Categories
-- ==============================
INSERT INTO Categories (CategoryName, Description, created_by)
VALUES
(N'Beverages', N'Soft drinks, coffees, teas, beers, and ales', N'system'),
(N'Condiments', N'Sweet and savory sauces, relishes, spreads, and seasonings', N'system'),
(N'Confections', N'Desserts, candies, and sweet breads', N'system'),
(N'Dairy Products', N'Cheeses', N'system'),
(N'Grains/Cereals', N'Breads, crackers, pasta, and cereal', N'system'),
(N'Meat/Poultry', N'Prepared meats', N'system'),
(N'Produce', N'Dried fruit and bean curd', N'system'),
(N'Seafood', N'Seaweed and fish', N'system');
GO

-- ==============================
-- Table: Customers
-- ==============================
CREATE TABLE Customers (
    CustomerID NVARCHAR(5) NOT NULL PRIMARY KEY,
    CompanyName NVARCHAR(40) NOT NULL,
    ContactName NVARCHAR(30) NULL,
    ContactTitle NVARCHAR(30) NULL,
    Address NVARCHAR(60) NULL,
    City NVARCHAR(15) NULL,
    Region NVARCHAR(15) NULL,
    PostalCode NVARCHAR(10) NULL,
    Country NVARCHAR(15) NULL,
    Phone NVARCHAR(24) NULL,
    Fax NVARCHAR(24) NULL,

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION
);
GO

-- ==============================
-- Seed data for Customers
-- ==============================
INSERT INTO Customers (CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, created_by)
VALUES
(N'ALFKI', N'Alfreds Futterkiste', N'Maria Anders', N'Sales Representative', N'Obere Str. 57', N'Berlin', NULL, N'12209', N'Germany', N'030-0074321', N'030-0076545', N'system'),
(N'ANATR', N'Ana Trujillo Emparedados y helados', N'Ana Trujillo', N'Owner', N'Avda. de la Constitución 2222', N'México D.F.', NULL, N'05021', N'Mexico', N'(5) 555-4729', N'(5) 555-3745', N'system'),
(N'ANTON', N'Antonio Moreno Taquería', N'Antonio Moreno', N'Owner', N'Mataderos 2312', N'México D.F.', NULL, N'05023', N'Mexico', N'(5) 555-3932', NULL, N'system'),
(N'AROUT', N'Around the Horn', N'Thomas Hardy', N'Sales Representative', N'120 Hanover Sq.', N'London', NULL, N'WA1 1DP', N'UK', N'(171) 555-7788', N'(171) 555-6750', N'system'),
(N'BERGS', N'Berglunds snabbköp', N'Christina Berglund', N'Order Administrator', N'Berguvsvägen 8', N'Luleå', NULL, N'S-958 22', N'Sweden', N'0921-12 34 65', N'0921-12 34 67', N'system'),
(N'BLAUS', N'Blauer See Delikatessen', N'Hanna Moos', N'Sales Representative', N'Forsterstr. 57', N'Mannheim', NULL, N'68306', N'Germany', N'0621-08460', N'0621-08924', N'system'),
(N'BLONP', N'Blondesddsl père et fils', N'Frédérique Citeaux', N'Marketing Manager', N'24, place Kléber', N'Strasbourg', NULL, N'67000', N'France', N'88.60.15.31', N'88.60.15.32', N'system'),
(N'BOLID', N'Bólido Comidas preparadas', N'Martín Sommer', N'Owner', N'C/ Araquil, 67', N'Madrid', NULL, N'28023', N'Spain', N'(91) 555 22 82', N'(91) 555 91 99', N'system'),
(N'BONAP', N'Bon app''', N'Laurence Lebihan', N'Owner', N'12, rue des Bouchers', N'Marseille', NULL, N'13008', N'France', N'91.24.45.40', N'91.24.45.41', N'system'),
(N'BOTTM', N'Bottom-Dollar Markets', N'Elizabeth Lincoln', N'Accounting Manager', N'23 Tsawassen Blvd.', N'Tsawassen', N'BC', N'T2F 8M4', N'Canada', N'(604) 555-4729', N'(604) 555-3745', N'system'),
(N'BSBEV', N'B''s Beverages', N'Victoria Ashworth', N'Sales Representative', N'Fauntleroy Circus', N'London', NULL, N'EC2 5NT', N'UK', N'(171) 555-1212', NULL, N'system'),
(N'CACTU', N'Cactus Comidas para llevar', N'Patricio Simpson', N'Sales Agent', N'Cerrito 333', N'Buenos Aires', NULL, N'1010', N'Argentina', N'(1) 135-5555', N'(1) 135-4892', N'system'),
(N'CENTC', N'Centro comercial Moctezuma', N'Francisco Chang', N'Marketing Manager', N'Sierras de Granada 9993', N'México D.F.', NULL, N'05022', N'Mexico', N'(5) 555-3392', N'(5) 555-7293', N'system'),
(N'CHOPS', N'Chop-suey Chinese', N'Yang Wang', N'Owner', N'Hauptstr. 29', N'Bern', NULL, N'3012', N'Switzerland', N'0452-076545', NULL, N'system'),
(N'COMMI', N'Comércio Mineiro', N'Pedro Afonso', N'Sales Associate', N'Av. dos Lusíadas, 23', N'Sao Paulo', N'SP', N'05432-043', N'Brazil', N'(11) 555-7647', NULL, N'system'),
(N'CONSH', N'Consolidated Holdings', N'Elizabeth Brown', N'Sales Representative', N'Berkeley Gardens 12 Brewery', N'London', NULL, N'WX1 6LT', N'UK', N'(171) 555-2282', N'(171) 555-9199', N'system'),
(N'DRACD', N'Drachenblut Delikatessen', N'Sven Ottlieb', N'Order Administrator', N'Walserweg 21', N'Aachen', NULL, N'52066', N'Germany', N'0241-039123', N'0241-059428', N'system'),
(N'DUMON', N'Du monde entier', N'Janine Labrune', N'Owner', N'67, rue des Cinquante Otages', N'Nantes', NULL, N'44000', N'France', N'40.67.88.88', N'40.67.89.89', N'system'),
(N'EASTC', N'Eastern Connection', N'Ann Devon', N'Sales Agent', N'35 King George', N'London', NULL, N'WX3 6FW', N'UK', N'(171) 555-0297', N'(171) 555-3373', N'system'),
(N'ERNSH', N'Ernst Handel', N'Roland Mendel', N'Sales Manager', N'Kirchgasse 6', N'Graz', NULL, N'8010', N'Austria', N'7675-3425', N'7675-3426', N'system'),
(N'FAMIA', N'Familia Arquibaldo', N'Aria Cruz', N'Marketing Assistant', N'Rua Orós, 92', N'Sao Paulo', N'SP', N'05442-030', N'Brazil', N'(11) 555-9857', NULL, N'system'),
(N'FISSA', N'FISSA Fabrica Inter. Salchichas S.A.', N'Diego Roel', N'Accounting Manager', N'C/ Moralzarzal, 86', N'Madrid', NULL, N'28034', N'Spain', N'(91) 555 94 44', N'(91) 555 55 93', N'system'),
(N'FOLIG', N'Folies gourmandes', N'Martine Rancé', N'Assistant Sales Agent', N'184, chaussée de Tournai', N'Lille', NULL, N'59000', N'France', N'20.16.10.16', N'20.16.10.17', N'system'),
(N'FOLKO', N'Folk och fä HB', N'Maria Larsson', N'Owner', N'Åkergatan 24', N'Bräcke', NULL, N'S-844 67', N'Sweden', N'0695-34 67 21', NULL, N'system'),
(N'FRANK', N'Frankenversand', N'Peter Franken', N'Marketing Manager', N'Berliner Platz 43', N'München', NULL, N'80805', N'Germany', N'089-0877310', N'089-0877451', N'system'),
(N'FRANR', N'France restauration', N'Carine Schmitt', N'Marketing Manager', N'54, rue Royale', N'Nantes', NULL, N'44000', N'France', N'40.32.21.21', N'40.32.21.20', N'system'),
(N'FRANS', N'Franchi S.p.A.', N'Paolo Accorti', N'Sales Representative', N'Via Monte Bianco 34', N'Torino', NULL, N'10100', N'Italy', N'011-4988260', N'011-4988261', N'system'),
(N'FURIB', N'Furia Bacalhau e Frutos do Mar', N'Lino Rodriguez', N'Sales Manager', N'Jardim das rosas n. 32', N'Lisboa', NULL, N'1675', N'Portugal', N'(1) 354-2534', N'(1) 354-2535', N'system'),
(N'GALED', N'Galería del gastrónomo', N'Eduardo Saavedra', N'Marketing Manager', N'Rambla de Cataluña, 23', N'Barcelona', NULL, N'08022', N'Spain', N'(93) 203 4560', N'(93) 203 4561', N'system'),
(N'GODOS', N'Godos Cocina Típica', N'José Pedro Freyre', N'Sales Manager', N'C/ Romero, 33', N'Sevilla', NULL, N'41101', N'Spain', N'(95) 555 82 82', NULL, N'system'),
(N'GOURL', N'Gourmet Lanchonetes', N'André Fonseca', N'Sales Associate', N'Av. Brasil, 442', N'Campinas', N'SP', N'04876-786', N'Brazil', N'(11) 555-9482', NULL, N'system'),
(N'GREAL', N'Great Lakes Food Market', N'Howard Snyder', N'Marketing Manager', N'2732 Baker Blvd.', N'Eugene', N'OR', N'97403', N'USA', N'(503) 555-7555', NULL, N'system'),
(N'GROSR', N'GROSELLA-Restaurante', N'Manuel Pereira', N'Owner', N'5ª Ave. Los Palos Grandes', N'Caracas', N'DF', N'1081', N'Venezuela', N'(2) 283-2951', N'(2) 283-3397', N'system'),
(N'HANAR', N'Hanari Carnes', N'Mario Pontes', N'Accounting Manager', N'Rua do Paço, 67', N'Rio de Janeiro', N'RJ', N'05454-876', N'Brazil', N'(21) 555-0091', N'(21) 555-8765', N'system'),
(N'HILAA', N'HILARION-Abastos', N'Carlos Hernández', N'Sales Representative', N'Carrera 22 con Ave. Carlos Soublette #8-35', N'San Cristóbal', N'Táchira', N'5022', N'Venezuela', N'(5) 555-1340', N'(5) 555-1948', N'system'),
(N'HUNGC', N'Hungry Coyote Import Store', N'Yoshi Latimer', N'Sales Representative', N'City Center Plaza 516 Main St.', N'Elgin', N'OR', N'97827', N'USA', N'(503) 555-6874', N'(503) 555-2376', N'system'),
(N'HUNGO', N'Hungry Owl All-Night Grocers', N'Patricia McKenna', N'Sales Associate', N'8 Johnstown Road', N'Cork', N'Co. Cork', NULL, N'Ireland', N'2967 542', N'2967 3333', N'system'),
(N'ISLAT', N'Island Trading', N'Helen Bennett', N'Marketing Manager', N'Garden House Crowther Way', N'Cowes', N'Isle of Wight', N'PO31 7PJ', N'UK', N'(198) 555-8888', NULL, N'system'),
(N'KOENE', N'Königlich Essen', N'Philip Cramer', N'Sales Associate', N'Maubelstr. 90', N'Brandenburg', NULL, N'14776', N'Germany', N'0555-09876', NULL, N'system'),
(N'LACOR', N'La corne d''abondance', N'Daniel Tonini', N'Sales Representative', N'67, avenue de l''Europe', N'Versailles', NULL, N'78000', N'France', N'30.59.84.10', N'30.59.85.11', N'system'),
(N'LAMAI', N'La maison d''Asie', N'Annette Roulet', N'Sales Manager', N'1 rue Alsace-Lorraine', N'Toulouse', NULL, N'31000', N'France', N'61.77.61.10', N'61.77.61.11', N'system'),
(N'LAUGB', N'Laughing Bacchus Wine Cellars', N'Yoshi Tannamuri', N'Marketing Assistant', N'1900 Oak St.', N'Vancouver', N'BC', N'V3F 2K1', N'Canada', N'(604) 555-3392', N'(604) 555-7293', N'system'),
(N'LAZYK', N'Lazy K Kountry Store', N'John Steel', N'Marketing Manager', N'12 Orchestra Terrace', N'Walla Walla', N'WA', N'99362', N'USA', N'(509) 555-7969', N'(509) 555-6221', N'system'),
(N'LEHMS', N'Lehmanns Marktstand', N'Renate Messner', N'Sales Representative', N'Magazinweg 7', N'Frankfurt a.M.', NULL, N'60528', N'Germany', N'069-0245984', N'069-0245874', N'system'),
(N'LETSS', N'Let''s Stop N Shop', N'Jaime Yorres', N'Owner', N'87 Polk St. Suite 5', N'San Francisco', N'CA', N'94117', N'USA', N'(415) 555-5938', NULL, N'system'),
(N'LILAS', N'LILA-Supermercado', N'Carlos González', N'Accounting Manager', N'Carrera 52 con Ave. Bolívar #65-98 Llano Largo', N'Barquisimeto', N'Lara', N'3508', N'Venezuela', N'(9) 331-6954', N'(9) 331-7256', N'system'),
(N'LINOD', N'LINO-Delicateses', N'Felipe Izquierdo', N'Owner', N'Ave. 5 de Mayo Porlamar', N'I. de Margarita', N'Nueva Esparta', N'4980', N'Venezuela', N'(8) 34-56-12', N'(8) 34-93-93', N'system'),
(N'LONEP', N'Lonesome Pine Restaurant', N'Fran Wilson', N'Sales Manager', N'89 Chiaroscuro Rd.', N'Portland', N'OR', N'97219', N'USA', N'(503) 555-9573', N'(503) 555-9646', N'system'),
(N'MAGAA', N'Magazzini Alimentari Riuniti', N'Giovanni Rovelli', N'Marketing Manager', N'Via Ludovico il Moro 22', N'Bergamo', NULL, N'24100', N'Italy', N'035-640230', N'035-640231', N'system'),
(N'MAISD', N'Maison Dewey', N'Catherine Dewey', N'Sales Agent', N'Rue Joseph-Bens 532', N'Bruxelles', NULL, N'B-1180', N'Belgium', N'(02) 201 24 67', N'(02) 201 24 68', N'system'),
(N'MEREP', N'Mère Paillarde', N'Jean Fresnière', N'Marketing Assistant', N'43 rue St. Laurent', N'Montréal', N'Québec', N'H1J 1C3', N'Canada', N'(514) 555-8054', N'(514) 555-8055', N'system'),
(N'MORGK', N'Morgenstern Gesundkost', N'Alexander Feuer', N'Marketing Assistant', N'Heerstr. 22', N'Leipzig', NULL, N'04179', N'Germany', N'0342-023176', NULL, N'system'),
(N'NORTS', N'North/South', N'Simon Crowther', N'Sales Associate', N'South House 300 Queensbridge', N'London', NULL, N'SW7 1RZ', N'UK', N'(171) 555-7733', N'(171) 555-2530', N'system'),
(N'OCEAN', N'Océano Atlántico Ltda.', N'Yvonne Moncada', N'Sales Agent', N'Ing. Gustavo Moncada 8585 Piso 20-A', N'Buenos Aires', NULL, N'1010', N'Argentina', N'(1) 135-5333', N'(1) 135-5535', N'system'),
(N'OTTIK', N'Ottilies Käseladen', N'Henriette Pfalzheim', N'Owner', N'Mehrheimerstr. 369', N'Köln', NULL, N'50739', N'Germany', N'0221-0644327', N'0221-0765721', N'system'),
(N'PARIS', N'Paris spécialités', N'Marie Bertrand', N'Owner', N'265, boulevard Charonne', N'Paris', NULL, N'75012', N'France', N'(1) 42.34.22.66', N'(1) 42.34.22.77', N'system'),
(N'PERIC', N'Pericles Comidas clásicas', N'Guillermo Fernández', N'Sales Representative', N'Calle Dr. Jorge Cash 321', N'México D.F.', NULL, N'05033', N'Mexico', N'(5) 552-3745', N'(5) 545-3745', N'system'),
(N'PICCO', N'Piccolo und mehr', N'Georg Pipps', N'Sales Manager', N'Geislweg 14', N'Salzburg', NULL, N'5020', N'Austria', N'6562-9722', N'6562-9723', N'system'),
(N'PRINI', N'Princesa Isabel Vinhos', N'Isabel de Castro', N'Sales Representative', N'Estrada da saúde n. 58', N'Lisboa', NULL, N'1756', N'Portugal', N'(1) 356-5634', NULL, N'system'),
(N'QUEDE', N'Que Delícia', N'Bernardo Batista', N'Accounting Manager', N'Rua da Panificadora, 12', N'Rio de Janeiro', N'RJ', N'02389-673', N'Brazil', N'(21) 555-4252', N'(21) 555-4545', N'system'),
(N'QUEEN', N'Queen Cozinha', N'Lúcia Carvalho', N'Marketing Assistant', N'Alameda dos Canàrios, 891', N'Sao Paulo', N'SP', N'05487-020', N'Brazil', N'(11) 555-1189', NULL, N'system'),
(N'QUICK', N'QUICK-Stop', N'Horst Kloss', N'Accounting Manager', N'Taucherstraße 10', N'Cunewalde', NULL, N'01307', N'Germany', N'0372-035188', NULL, N'system'),
(N'RANCH', N'Rancho grande', N'Sergio Gutiérrez', N'Sales Representative', N'Av. del Libertador 900', N'Buenos Aires', NULL, N'1010', N'Argentina', N'(1) 123-5555', N'(1) 123-5556', N'system'),
(N'RATTC', N'Rattlesnake Canyon Grocery', N'Paula Wilson', N'Assistant Sales Representative', N'2817 Milton Dr.', N'Albuquerque', N'NM', N'87110', N'USA', N'(505) 555-5939', N'(505) 555-3620', N'system'),
(N'REGGC', N'Reggiani Caseifici', N'Maurizio Moroni', N'Sales Associate', N'Strada Provinciale 124', N'Reggio Emilia', NULL, N'42100', N'Italy', N'0522-556721', N'0522-556722', N'system'),
(N'RICAR', N'Ricardo Adocicados', N'Janete Limeira', N'Assistant Sales Agent', N'Av. Copacabana, 267', N'Rio de Janeiro', N'RJ', N'02389-890', N'Brazil', N'(21) 555-3412', NULL, N'system'),
(N'RICSU', N'Richter Supermarkt', N'Michael Holz', N'Sales Manager', N'Grenzacherweg 237', N'Genève', NULL, N'1203', N'Switzerland', N'0897-034214', NULL, N'system'),
(N'ROMEY', N'Romero y tomillo', N'Alejandra Camino', N'Accounting Manager', N'Gran Vía, 1', N'Madrid', NULL, N'28001', N'Spain', N'(91) 745 6200', N'(91) 745 6210', N'system'),
(N'SANTG', N'Santé Gourmet', N'Jonas Bergulfsen', N'Owner', N'Erling Skakkes gate 78', N'Stavern', NULL, N'4110', N'Norway', N'07-98 92 35', N'07-98 92 47', N'system'),
(N'SAVEA', N'Save-a-lot Markets', N'Jose Pavarotti', N'Sales Representative', N'187 Suffolk Ln.', N'Boise', N'ID', N'83720', N'USA', N'(208) 555-8097', NULL, N'system'),
(N'SEVES', N'Seven Seas Imports', N'Hari Kumar', N'Sales Manager', N'90 Wadhurst Rd.', N'London', NULL, N'OX15 4NB', N'UK', N'(171) 555-1717', N'(171) 555-5646', N'system'),
(N'SIMOB', N'Simons bistro', N'Jytte Petersen', N'Owner', N'Vinbæltet 34', N'Kobenhavn', NULL, N'1734', N'Denmark', N'31 12 34 56', N'31 13 35 57', N'system'),
(N'SPECD', N'Spécialités du monde', N'Dominique Perrier', N'Marketing Manager', N'25, rue Lauriston', N'Paris', NULL, N'75016', N'France', N'(1) 47.55.60.10', N'(1) 47.55.60.20', N'system'),
(N'SPLIR', N'Split Rail Beer & Ale', N'Art Braunschweiger', N'Sales Manager', N'P.O. Box 555', N'Lander', N'WY', N'82520', N'USA', N'(307) 555-4680', N'(307) 555-6525', N'system'),
(N'SUPRD', N'Suprêmes délices', N'Pascale Cartrain', N'Accounting Manager', N'Boulevard Tirou, 255', N'Charleroi', NULL, N'B-6000', N'Belgium', N'(071) 23 67 22 20', N'(071) 23 67 22 21', N'system'),
(N'THEBI', N'The Big Cheese', N'Liz Nixon', N'Marketing Manager', N'89 Jefferson Way Suite 2', N'Portland', N'OR', N'97201', N'USA', N'(503) 555-3612', NULL, N'system'),
(N'THECR', N'The Cracker Box', N'Liu Wong', N'Marketing Assistant', N'55 Grizzly Peak Rd.', N'Butte', N'MT', N'59801', N'USA', N'(406) 555-5834', N'(406) 555-8083', N'system'),
(N'TOMSP', N'Toms Spezialitäten', N'Karin Josephs', N'Marketing Manager', N'Luisenstr. 48', N'Münster', NULL, N'44087', N'Germany', N'0251-031259', N'0251-035695', N'system'),
(N'TORTU', N'Tortuga Restaurante', N'Miguel Angel Paolino', N'Owner', N'Avda. Azteca 123', N'México D.F.', NULL, N'05033', N'Mexico', N'(5) 555-2933', NULL, N'system'),
(N'TRADH', N'Tradição Hipermercados', N'Anabela Domingues', N'Sales Representative', N'Av. Inês de Castro, 414', N'Sao Paulo', N'SP', N'05634-030', N'Brazil', N'(11) 555-2167', N'(11) 555-2168', N'system'),
(N'TRAIH', N'Trail''s Head Gourmet Provisioners', N'Helvetius Nagy', N'Sales Associate', N'722 DaVinci Blvd.', N'Kirkland', N'WA', N'98034', N'USA', N'(206) 555-8257', N'(206) 555-2174', N'system'),
(N'VAFFE', N'Vaffeljernet', N'Palle Ibsen', N'Sales Manager', N'Smagsloget 45', N'Århus', NULL, N'8200', N'Denmark', N'86 21 32 43', N'86 22 33 44', N'system'),
(N'VICTE', N'Victuailles en stock', N'Mary Saveley', N'Sales Agent', N'2, rue du Commerce', N'Lyon', NULL, N'69004', N'France', N'78.32.54.86', N'78.32.54.87', N'system'),
(N'VINET', N'Vins et alcools Chevalier', N'Paul Henriot', N'Accounting Manager', N'59 rue de l''Abbaye', N'Reims', NULL, N'51100', N'France', N'26.47.15.10', N'26.47.15.11', N'system'),
(N'WANDK', N'Die Wandernde Kuh', N'Rita Müller', N'Sales Representative', N'Adenauerallee 900', N'Stuttgart', NULL, N'70563', N'Germany', N'0711-020361', N'0711-035428', N'system'),
(N'WARTH', N'Wartian Herkku', N'Pirkko Koskitalo', N'Accounting Manager', N'Torikatu 38', N'Oulu', NULL, N'90110', N'Finland', N'981-443655', N'981-443655', N'system'),
(N'WELLI', N'Wellington Importadora', N'Paula Parente', N'Sales Manager', N'Rua do Mercado, 12', N'Resende', N'SP', N'08737-363', N'Brazil', N'(14) 555-8122', NULL, N'system'),
(N'WHITC', N'White Clover Markets', N'Karl Jablonski', N'Owner', N'305 - 14th Ave. S. Suite 3B', N'Seattle', N'WA', N'98128', N'USA', N'(206) 555-4112', N'(206) 555-4115', N'system'),
(N'WILMK', N'Wilman Kala', N'Matti Karttunen', N'Owner/Marketing Assistant', N'Keskuskatu 45', N'Helsinki', NULL, N'21240', N'Finland', N'90-224 8858', N'90-224 8858', N'system'),
(N'WOLZA', N'Wolski Zajazd', N'Zbyszek Piestrzeniewicz', N'Owner', N'ul. Filtrowa 68', N'Warszawa', NULL, N'01-012', N'Poland', N'(26) 642-7012', N'(26) 642-7012', N'system');
GO

-- ==============================
-- Table: CustomerDemographics
-- ==============================
CREATE TABLE CustomerDemographics (
    CustomerTypeID NVARCHAR(5) PRIMARY KEY,
    CustomerDesc NVARCHAR(MAX) NULL,
   
   -- BaseAuditable fields
   created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
   created_by NVARCHAR(50) NOT NULL,
   updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
   updated_by NVARCHAR(50) NULL,
   row_version ROWVERSION
);
GO

-- Junction: CustomerCustomerDemo
CREATE TABLE CustomerCustomerDemo (
    CustomerID NVARCHAR(5) NOT NULL,
    CustomerTypeID NVARCHAR(5) NOT NULL,
 
    CONSTRAINT PK_CustomerCustomerDemo PRIMARY KEY (CustomerID, CustomerTypeID),
    CONSTRAINT FK_CustCustDemo_Customers FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    CONSTRAINT FK_CustCustDemo_CustDemo FOREIGN KEY (CustomerTypeID) REFERENCES CustomerDemographics(CustomerTypeID)
);
GO

-- ==============================
-- Table: Employees
-- ==============================
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LastName NVARCHAR(20) NOT NULL,
    FirstName NVARCHAR(10) NOT NULL,
    Title NVARCHAR(30) NULL,
    TitleOfCourtesy NVARCHAR(25) NULL,
    BirthDate DATETIME NULL,
    HireDate DATETIME NULL,
    Address NVARCHAR(60) NULL,
    City NVARCHAR(15) NULL,
    Region NVARCHAR(15) NULL,
    PostalCode NVARCHAR(10) NULL,
    Country NVARCHAR(15) NULL,
    HomePhone NVARCHAR(24) NULL,
    Extension NVARCHAR(4) NULL,
    Photo VARBINARY(MAX) NULL,
    Notes NVARCHAR(MAX) NULL,
    ReportsTo INT NULL,
    PhotoPath NVARCHAR(255) NULL,

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION,

    CONSTRAINT FK_Employees_Employees FOREIGN KEY (ReportsTo) REFERENCES Employees(EmployeeID)
);
GO

-- ==============================
-- Seed data for Customers
-- ==============================
INSERT INTO Employees (LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Notes, ReportsTo, PhotoPath, created_by)
VALUES
(N'Davolio', N'Nancy', N'Sales Representative', N'Ms.', '1948-12-08', '1992-05-01', N'507 - 20th Ave. E. Apt. 2A', N'Seattle', N'WA', N'98122', N'USA', N'(206) 555-9857', N'5467', N'Education includes a BA in psychology from Colorado State University.', NULL, N'http://accweb/emmployees/davolio.bmp', N'system'),
(N'Fuller', N'Andrew', N'Vice President, Sales', N'Dr.', '1952-02-19', '1992-08-14', N'908 W. Capital Way', N'Tacoma', N'WA', N'98401', N'USA', N'(206) 555-9482', N'3457', N'Andrew received his BTS commercial and a Ph.D. in international marketing.', NULL, N'http://accweb/emmployees/fuller.bmp', N'system'),
(N'Leverling', N'Janet', N'Sales Representative', N'Ms.', '1963-08-30', '1992-04-01', N'722 Moss Bay Blvd.', N'Kirkland', N'WA', N'98033', N'USA', N'(206) 555-3412', N'3355', N'Janet has a BS degree in chemistry from Boston College.', 2, N'http://accweb/emmployees/leverling.bmp', N'system'),
(N'Peacock', N'Margaret', N'Sales Representative', N'Mrs.', '1937-09-19', '1993-05-03', N'4110 Old Redmond Rd.', N'Redmond', N'WA', N'98052', N'USA', N'(206) 555-8122', N'5176', N'Margaret holds a BA in English literature from Concordia College.', 2, N'http://accweb/emmployees/peacock.bmp', N'system'),
(N'Buchanan', N'Steven', N'Sales Manager', N'Mr.', '1955-03-04', '1993-10-17', N'14 Garrett Hill', N'London', NULL, N'SW1 8JR', N'UK', N'(71) 555-4848', N'3453', N'Steven Buchanan graduated from St. Andrews University.', 2, N'http://accweb/emmployees/buchanan.bmp', N'system'),
(N'Suyama', N'Michael', N'Sales Representative', N'Mr.', '1963-07-02', '1993-10-17', N'Coventry House Miner Rd.', N'London', NULL, N'EC2 7JR', N'UK', N'(71) 555-7773', N'428', N'Michael is a graduate of Sussex University (MA, economics).', 5, N'http://accweb/emmployees/suyama.bmp', N'system'),
(N'King', N'Robert', N'Sales Representative', N'Mr.', '1960-05-29', '1994-01-02', N'Edgeham Hollow Winchester Way', N'London', NULL, N'RG1 9SP', N'UK', N'(71) 555-5598', N'465', N'Robert King served in the Peace Corps and traveled extensively.', 5, N'http://accweb/emmployees/king.bmp', N'system'),
(N'Callahan', N'Laura', N'Inside Sales Coordinator', N'Ms.', '1958-01-09', '1994-03-05', N'4726 - 11th Ave. N.E.', N'Seattle', N'WA', N'98105', N'USA', N'(206) 555-1189', N'2344', N'Laura received a BA in psychology from the University of Washington.', 2, N'http://accweb/emmployees/callahan.bmp', N'system'),
(N'Dodsworth', N'Anne', N'Sales Representative', N'Ms.', '1966-01-27', '1994-11-15', N'7 Houndstooth Rd.', N'London', NULL, N'WG2 7LT', N'UK', N'(71) 555-4444', N'452', N'Anne has a BA degree in English from St. Lawrence College.', 5, N'http://accweb/emmployees/dodsworth.bmp', N'system');
GO

-- ==============================
-- Table: Region
-- ==============================
CREATE TABLE Region (
    RegionID INT NOT NULL PRIMARY KEY,
    RegionDescription NVARCHAR(50) NOT NULL,

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION
);
GO

-- ==============================
-- Seed data for Region
-- ==============================
INSERT INTO Region (RegionID, RegionDescription, created_by)
VALUES
(1, N'Eastern', 'system'),
(2, N'Western', 'system'),
(3, N'Northern', 'system'),
(4, N'Southern', 'system');
GO

-- ==============================
-- Table: Territories
-- ==============================
CREATE TABLE Territories (
    TerritoryID NVARCHAR(20) PRIMARY KEY,
    TerritoryDescription NVARCHAR(50) NOT NULL,
    RegionID INT NOT NULL,
   
    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION,
      
    CONSTRAINT FK_Territories_Region FOREIGN KEY (RegionID) REFERENCES Region(RegionID)
);
GO

-- ==============================
-- Seed data for Territories
-- ==============================
INSERT INTO Territories (TerritoryID, TerritoryDescription, RegionID, created_by)
VALUES 
('01581','Westboro',1, 'system'),
('01730','Bedford',1, 'system'),
('01833','Georgetow',1, 'system'),
('02116','Boston',1, 'system'),
('02139','Cambridge',1, 'system'),
('02184','Braintree',1, 'system'),
('02903','Providence',1, 'system'),
('03049','Hollis',3, 'system'),
('03801','Portsmouth',3, 'system'),
('06897','Wilton',1, 'system'),
('07960','Morristown',1, 'system'),
('08837','Edison',1, 'system'),
('10019','New York',1, 'system'),
('10038','New York',1, 'system'),
('11747','Mellvile',1, 'system'),
('14450','Fairport',1, 'system'),
('19428','Philadelphia',3, 'system'),
('19713','Neward',1, 'system'),
('20852','Rockville',1, 'system'),
('27403','Greensboro',1, 'system'),
('27511','Cary',1, 'system'),
('29202','Columbia',4, 'system'),
('30346','Atlanta',4, 'system'),
('31406','Savannah',4, 'system'),
('32859','Orlando',4, 'system'),
('33607','Tampa',4, 'system'),
('40222','Louisville',1, 'system'),
('44122','Beachwood',3, 'system'),
('45839','Findlay',3, 'system'),
('48075','Southfield',3, 'system'),
('48084','Troy',3, 'system'),
('48304','Bloomfield Hills',3, 'system'),
('53404','Racine',3, 'system'),
('55113','Roseville',3, 'system'),
('55439','Minneapolis',3, 'system'),
('60179','Hoffman Estates',2, 'system'),
('60601','Chicago',2, 'system'),
('72716','Bentonville',4, 'system'),
('75234','Dallas',4, 'system'),
('78759','Austin',4, 'system'),
('80202','Denver',2, 'system'),
('80909','Colorado Springs',2, 'system'),
('85014','Phoenix',2, 'system'),
('85251','Scottsdale',2, 'system'),
('90405','Santa Monica',2, 'system'),
('94025','Menlo Park',2, 'system'),
('94105','San Francisco',2, 'system'),
('95008','Campbell',2, 'system'),
('95054','Santa Clara',2, 'system'),
('95060','Santa Cruz',2, 'system'),
('98004','Bellevue',2, 'system'),
('98052','Redmond',2, 'system'),
('98104','Seattle',2, 'system');
GO

-- Junction: EmployeeTerritories
CREATE TABLE EmployeeTerritories (
    EmployeeID INT NOT NULL,
    TerritoryID NVARCHAR(20) NOT NULL,
    
    CONSTRAINT PK_EmployeeTerritories PRIMARY KEY (EmployeeID, TerritoryID),
    CONSTRAINT FK_EmpTerr_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    CONSTRAINT FK_EmpTerr_Territories FOREIGN KEY (TerritoryID) REFERENCES Territories(TerritoryID)
);
GO

-- ==============================
-- Table: Shippers
-- ==============================
CREATE TABLE Shippers (
    ShipperID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyName NVARCHAR(40) NOT NULL,
    Phone NVARCHAR(24) NULL,

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION
);
GO

-- ==============================
-- Seed data for Shippers
-- ==============================
INSERT INTO Shippers (CompanyName, Phone, created_by)
VALUES
(N'Speedy Express', N'(503) 555-9831', N'system'),
(N'United Package', N'(503) 555-3199', N'system'),
(N'Federal Shipping', N'(503) 555-9931', N'system');
GO

-- ==============================
-- Table: Suppliers
-- ==============================
CREATE TABLE Suppliers (
    SupplierID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyName NVARCHAR(40) NOT NULL,
    ContactName NVARCHAR(30) NULL,
    ContactTitle NVARCHAR(30) NULL,
    Address NVARCHAR(60) NULL,
    City NVARCHAR(15) NULL,
    Region NVARCHAR(15) NULL,
    PostalCode NVARCHAR(10) NULL,
    Country NVARCHAR(15) NULL,
    Phone NVARCHAR(24) NULL,
    Fax NVARCHAR(24) NULL,
    HomePage NVARCHAR(MAX) NULL,

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION
);
GO

-- ==============================
-- Seed data for Suppliers
-- ==============================
INSERT INTO Suppliers (CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, created_by)
VALUES
(N'Exotic Liquids', N'Charlotte Cooper', N'Purchasing Manager', N'49 Gilbert St.', N'London', NULL, N'EC1 4SD', N'UK', N'(171) 555-2222', N'system'),
(N'New Orleans Cajun Delights', N'Shelley Burke', N'Order Administrator', N'P.O. Box 78934', N'New Orleans', N'LA', N'70117', N'USA', N'(100) 555-4822', N'system'),
(N'Grandma Kelly''s Homestead', N'Regina Murphy', N'Sales Representative', N'707 Oxford Rd.', N'Ann Arbor', N'MI', N'48104', N'USA', N'(313) 555-5735', N'system'),
(N'Tokyo Traders', N'Yoshi Nagase', N'Marketing Manager', N'9-8 Sekimai Musashino-shi', N'Tokyo', NULL, N'100', N'Japan', N'(03) 3555-5011', N'system'),
(N'Cooperativa de Quesos ''Las Cabras''', N'Antonio del Valle Saavedra', N'Export Administrator', N'Calle del Rosal 4', N'Oviedo', N'Asturias', N'33007', N'Spain', N'(98) 598 76 54', N'system'),
(N'Mayumi''s', N'Mayumi Ohno', N'Marketing Representative', N'92 Setsuko Chuo-ku', N'Osaka', NULL, N'545', N'Japan', N'(06) 431-7877', N'system'),
(N'Pavlova, Ltd.', N'Ian Devling', N'Marketing Manager', N'74 Rose St. Moonie Ponds', N'Melbourne', N'Victoria', N'3058', N'Australia', N'(03) 444-2343', N'system'),
(N'Specialty Biscuits, Ltd.', N'Peter Wilson', N'Sales Representative', N'29 King''s Way', N'Manchester', NULL, N'M14 GSD', N'UK', N'(161) 555-4448', N'system'),
(N'PB Knäckebröd AB', N'Lars Peterson', N'Sales Agent', N'Kaloadagatan 13', N'Göteborg', NULL, N'S-345 67', N'Sweden', N'031-987 65 43', N'system'),
(N'Refrescos Americanas LTDA', N'Carlos Diaz', N'Marketing Manager', N'Av. das Americanas 12.890', N'Sao Paulo', NULL, N'5442', N'Brazil', N'(11) 555 4640', N'system'),
(N'Heli Süßwaren GmbH & Co. KG', N'Petra Winkler', N'Sales Manager', N'Tiergartenstraße 5', N'Berlin', NULL, N'10785', N'Germany', N'(010) 9984510', N'system'),
(N'Plutzer Lebensmittelgroßmärkte AG', N'Martin Bein', N'International Marketing Mgr.', N'Bogenallee 51', N'Frankfurt', NULL, N'60439', N'Germany', N'(069) 992755', N'system'),
(N'Nord-Ost-Fisch Handelsgesellschaft mbH', N'Sven Petersen', N'Coordinator Foreign Markets', N'Frahmredder 112a', N'Cuxhaven', NULL, N'27478', N'Germany', N'(04721) 8713', N'system'),
(N'Formaggi Fortini s.r.l.', N'Elio Rossi', N'Sales Representative', N'Viale Dante, 75', N'Ravenna', NULL, N'48100', N'Italy', N'(0544) 60323', N'system'),
(N'Norske Meierier', N'Beate Vileid', N'Marketing Manager', N'Hatlevegen 5', N'Sandvika', NULL, N'1320', N'Norway', N'(0)2-953010', N'system'),
(N'Bigfoot Breweries', N'Cheryl Saylor', N'Regional Account Rep.', N'3400 - 8th Avenue Suite 210', N'Bend', N'OR', N'97101', N'USA', N'(503) 555-9931', N'system'),
(N'Svensk Sjöfuda AB', N'Michael Björn', N'Sales Representative', N'Brovallavägen 231', N'Stockholm', NULL, N'S-123 45', N'Sweden', N'08-123 45 67', N'system'),
(N'Aux joyeux ecclésiastiques', N'Guylène Nodier', N'Sales Manager', N'203, Rue des Francs-Bourgeois', N'Paris', NULL, N'75004', N'France', N'(1) 03.83.00.68', N'system'),
(N'New England Seafood Cannery', N'Robb Merchant', N'Wholesale Account Agent', N'Order Processing Dept. 2100 Paul Revere Blvd.', N'Boston', N'MA', N'02134', N'USA', N'(617) 555-3267', N'system'),
(N'Leka Trading', N'Chandra Leka', N'Owner', N'471 Serangoon Loop, Suite #402', N'Singapore', NULL, N'0512', N'Singapore', N'555-8787', N'system'),
(N'Lyngbysild', N'Niels Petersen', N'Sales Manager', N'Lyngbysild Fiskebakken 10', N'Lyngby', NULL, N'2800', N'Denmark', N'43844108', N'system'),
(N'Zaanse Snoepfabriek', N'Dirk Luchte', N'Accounting Manager', N'Verkoop Rijnweg 22', N'Zaandam', NULL, N'9999 ZZ', N'Netherlands', N'(12345) 1212', N'system'),
(N'Karkki Oy', N'Anne Heikkonen', N'Product Manager', N'Valtakatu 12', N'Lappeenranta', NULL, N'53120', N'Finland', N'(953) 10956', N'system'),
(N'G''day, Mate', N'Wendy Mackenzie', N'Sales Representative', N'170 Prince Edward Parade Hunter''s Hill', N'Sydney', N'NSW', N'2042', N'Australia', N'(02) 555-5914', N'system'),
(N'Ma Maison', N'Jean-Guy Lauzon', N'Marketing Manager', N'2960 Rue St. Laurent', N'Montréal', N'Québec', N'H1J 1C3', N'Canada', N'(514) 555-9022', N'system'),
(N'Pasta Buttini s.r.l.', N'Giovanni Giudici', N'Order Administrator', N'Via dei Gelsomini, 153', N'Salerno', NULL, N'84100', N'Italy', N'(089) 6547665', N'system'),
(N'Escargots Nouveaux', N'Marie Delamare', N'Sales Manager', N'22, rue H. Voiron', N'Montceau', NULL, N'71300', N'France', N'85.57.00.07', N'system'),
(N'Gai pâturage', N'Eliane Noz', N'Sales Representative', N'Bat. B 3, rue des Alpes', N'Annecy', NULL, N'74000', N'France', N'38.76.98.06', N'system'),
(N'Forêts d''érables', N'Chantal Goulet', N'Accounting Manager', N'148 rue Chasseur', N'Ste-Hyacinthe', N'Québec', N'J2S 7S8', N'Canada', N'(514) 555-2955', N'system');
GO

-- ==============================
-- Table: Products
-- ==============================
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductName NVARCHAR(40) NOT NULL,
    SupplierID INT NULL,
    CategoryID INT NULL,
    QuantityPerUnit NVARCHAR(20) NULL,
    UnitPrice MONEY NULL DEFAULT 0,
    UnitsInStock SMALLINT NULL DEFAULT 0,
    UnitsOnOrder SMALLINT NULL DEFAULT 0,
    ReorderLevel SMALLINT NULL DEFAULT 0,
    Discontinued BIT NOT NULL DEFAULT 0,

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION,

    CONSTRAINT FK_Products_Suppliers FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);
GO

-- ==============================
-- Seed data for Products
-- ==============================
INSERT INTO Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued, created_by)
VALUES
(N'Chai', 1, 1, N'10 boxes x 20 bags', 18, 39, 0, 10, 0, N'system'),
(N'Chang', 1, 1, N'24 - 12 oz bottles', 19, 17, 40, 25, 0, N'system'),
(N'Aniseed Syrup', 1, 2, N'12 - 550 ml bottles', 10, 13, 70, 25, 0, N'system'),
(N'Chef Anton''s Cajun Seasoning', 2, 2, N'48 - 6 oz jars', 22, 53, 0, 0, 0, N'system'),
(N'Chef Anton''s Gumbo Mix', 2, 2, N'36 boxes', 21.35, 0, 0, 0, 1, N'system'),
(N'Grandma''s Boysenberry Spread', 3, 2, N'12 - 8 oz jars', 25, 120, 0, 25, 0, N'system'),
(N'Uncle Bob''s Organic Dried Pears', 3, 7, N'12 - 1 lb pkgs.', 30, 15, 0, 10, 0, N'system'),
(N'Northwoods Cranberry Sauce', 3, 2, N'12 - 12 oz jars', 40, 6, 0, 0, 0, N'system'),
(N'Mishi Kobe Niku', 4, 6, N'18 - 500 g pkgs.', 97, 29, 0, 0, 1, N'system'),
(N'Ikura', 4, 8, N'12 - 200 ml jars', 31, 31, 0, 0, 0, N'system'),
(N'Queso Cabrales', 5, 4, N'1 kg pkg.', 21, 22, 30, 30, 0, N'system'),
(N'Queso Manchego La Pastora', 5, 4, N'10 - 500 g pkgs.', 38, 86, 0, 0, 0, N'system'),
(N'Konbu', 6, 8, N'2 kg box', 6, 24, 0, 5, 0, N'system'),
(N'Tofu', 6, 7, N'40 - 100 g pkgs.', 23.25, 35, 0, 0, 0, N'system'),
(N'Genen Shouyu', 6, 2, N'24 - 250 ml bottles', 15.5, 39, 0, 5, 0, N'system'),
(N'Pavlova', 7, 3, N'32 - 500 g boxes', 17.45, 29, 0, 10, 0, N'system'),
(N'Alice Mutton', 7, 6, N'20 - 1 kg tins', 39, 0, 0, 0, 1, N'system'),
(N'Carnarvon Tigers', 7, 8, N'16 kg pkg.', 62.5, 42, 0, 0, 0, N'system'),
(N'Teatime Chocolate Biscuits', 8, 3, N'10 boxes x 12 pieces', 9.2, 25, 0, 5, 0, N'system'),
(N'Sir Rodney''s Marmalade', 8, 3, N'30 gift boxes', 81, 40, 0, 0, 0, N'system'),
(N'Sir Rodney''s Scones', 8, 3, N'24 pkgs. x 4 pieces', 10, 3, 40, 5, 0, N'system'),
(N'Gustaf''s Knäckebröd', 9, 5, N'24 - 500 g pkgs.', 21, 104, 0, 25, 0, N'system'),
(N'Tunnbröd', 9, 5, N'12 - 250 g pkgs.', 9, 61, 0, 25, 0, N'system'),
(N'Guaraná Fantástica', 10, 1, N'12 - 355 ml cans', 4.5, 20, 0, 0, 1, N'system'),
(N'NuNuCa Nuß-Nougat-Creme', 11, 3, N'20 - 450 g glasses', 14, 76, 0, 30, 0, N'system'),
(N'Gumbär Gummibärchen', 11, 3, N'100 - 250 g bags', 31.23, 15, 0, 0, 0, N'system'),
(N'Schoggi Schokolade', 11, 3, N'100 - 100 g pieces', 43.9, 49, 0, 30, 0, N'system'),
(N'Rössle Sauerkraut', 12, 7, N'25 - 825 g cans', 45.6, 26, 0, 0, 1, N'system'),
(N'Thüringer Rostbratwurst', 12, 6, N'50 bags x 30 sausgs.', 123.79, 0, 0, 0, 1, N'system'),
(N'Nord-Ost Matjeshering', 13, 8, N'10 - 200 g glasses', 25.89, 10, 0, 15, 0, N'system'),
(N'Gorgonzola Telino', 14, 4, N'12 - 100 g pkgs', 12.5, 0, 70, 20, 0, N'system'),
(N'Mascarpone Fabioli', 14, 4, N'24 - 200 g pkgs.', 32, 9, 40, 25, 0, N'system'),
(N'Geitost', 15, 4, N'500 g', 2.5, 112, 0, 20, 0, N'system'),
(N'Sasquatch Ale', 16, 1, N'24 - 12 oz bottles', 14, 111, 0, 15, 0, N'system'),
(N'Steeleye Stout', 16, 1, N'24 - 12 oz bottles', 18, 20, 0, 15, 0, N'system'),
(N'Inlagd Sill', 17, 8, N'24 - 250 g jars', 19, 112, 0, 20, 0, N'system'),
(N'Gravad lax', 17, 8, N'12 - 500 g pkgs.', 26, 11, 50, 25, 0, N'system'),
(N'Côte de Blaye', 18, 1, N'12 - 75 cl bottles', 263.5, 17, 0, 15, 0, N'system'),
(N'Chartreuse verte', 18, 1, N'750 cc per bottle', 18, 69, 0, 5, 0, N'system'),
(N'Boston Crab Meat', 19, 8, N'24 - 4 oz tins', 18.4, 123, 0, 30, 0, N'system'),
(N'Jack''s New England Clam Chowder', 19, 8, N'12 - 12 oz cans', 9.65, 85, 0, 10, 0, N'system'),
(N'Singaporean Hokkien Fried Mee', 20, 5, N'32 - 1 kg pkgs.', 14, 26, 0, 0, 1, N'system'),
(N'Ipoh Coffee', 20, 1, N'16 - 500 g tins', 46, 17, 10, 25, 0, N'system'),
(N'Gula Malacca', 20, 2, N'20 - 2 kg bags', 19.45, 27, 0, 15, 0, N'system'),
(N'Rogede sild', 21, 8, N'1k pkg.', 9.5, 5, 70, 15, 0, N'system'),
(N'Spegesild', 21, 8, N'4 - 450 g glasses', 12, 95, 0, 0, 0, N'system'),
(N'Zaanse koeken', 22, 3, N'10 - 4 oz boxes', 9.5, 36, 0, 0, 0, N'system'),
(N'Chocolade', 22, 3, N'10 pkgs.', 12.75, 15, 70, 25, 0, N'system'),
(N'Maxilaku', 23, 3, N'24 - 50 g pkgs.', 20, 10, 60, 15, 0, N'system'),
(N'Valkoinen suklaa', 23, 3, N'12 - 100 g bars', 16.25, 65, 0, 30, 0, N'system'),
(N'Manjimup Dried Apples', 24, 7, N'50 - 300 g pkgs.', 53, 20, 0, 10, 0, N'system'),
(N'Filo Mix', 24, 5, N'16 - 2 kg boxes', 7, 38, 0, 25, 0, N'system'),
(N'Perth Pasties', 24, 6, N'48 pieces', 32.8, 0, 0, 0, 1, N'system'),
(N'Tourtière', 25, 6, N'16 pies', 7.45, 21, 0, 10, 0, N'system'),
(N'Pâté chinois', 25, 6, N'24 boxes x 2 pies', 24, 115, 0, 20, 0, N'system'),
(N'Gnocchi di nonna Alice', 26, 5, N'24 - 250 g pkgs.', 38, 21, 10, 30, 0, N'system'),
(N'Ravioli Angelo', 26, 5, N'24 - 250 g pkgs.', 19.5, 36, 0, 20, 0, N'system'),
(N'Escargots de Bourgogne', 27, 8, N'24 pieces', 13.25, 62, 0, 20, 0, N'system'),
(N'Raclette Courdavault', 28, 4, N'5 kg pkg.', 55, 79, 0, 0, 0, N'system'),
(N'Camembert Pierrot', 28, 4, N'15 - 300 g rounds', 34, 19, 0, 0, 0, N'system'),
(N'Sirop d''érable', 29, 2, N'24 - 500 ml bottles', 28.5, 113, 0, 25, 0, N'system'),
(N'Tarte au sucre', 29, 3, N'48 pies', 49.3, 17, 0, 0, 0, N'system'),
(N'Vegie-spread', 7, 2, N'15 - 625 g jars', 43.9, 24, 0, 5, 0, N'system'),
(N'Wimmers gute Semmelknödel', 12, 5, N'20 bags x 4 pieces', 33.25, 22, 80, 30, 0, N'system'),
(N'Louisiana Fiery Hot Pepper Sauce', 2, 2, N'32 - 8 oz bottles', 21.05, 76, 0, 0, 0, N'system'),
(N'Louisiana Hot Spiced Okra', 2, 2, N'24 - 8 oz jars', 17, 4, 100, 20, 0, N'system'),
(N'Laughing Lumberjack Lager', 16, 1, N'24 - 12 oz bottles', 14, 52, 0, 10, 0, N'system'),
(N'Scottish Longbreads', 8, 3, N'10 boxes x 8 pieces', 12.5, 6, 10, 15, 0, N'system'),
(N'Gudbrandsdalsost', 15, 4, N'10 kg pkg.', 36, 26, 0, 15, 0, N'system'),
(N'Outback Lager', 7, 1, N'24 - 355 ml bottles', 15, 15, 10, 30, 0, N'system'),
(N'Flotemysost', 15, 4, N'10 - 500 g pkgs.', 21.5, 26, 0, 0, 0, N'system'),
(N'Mozzarella di Giovanni', 14, 4, N'24 - 200 g pkgs.', 34.8, 14, 0, 0, 0, N'system'),
(N'Röd Kaviar', 17, 8, N'24 - 150 g jars', 15, 101, 0, 5, 0, N'system'),
(N'Longlife Tofu', 4, 7, N'5 kg pkg.', 10, 4, 20, 5, 0, N'system'),
(N'Rhönbräu Klosterbier', 12, 1, N'24 - 0.5 l bottles', 7.75, 125, 0, 25, 0, N'system'),
(N'Lakkalikööri', 23, 1, N'500 ml', 18, 57, 0, 20, 0, N'system'),
(N'Original Frankfurter grüne Soße', 12, 2, N'12 boxes', 13, 32, 0, 15, 0, N'system');
GO

-- ==============================
-- Table: Orders
-- ==============================
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerID NVARCHAR(5) NULL,
    EmployeeID INT NULL,
    OrderDate DATETIME NULL,
    RequiredDate DATETIME NULL,
    ShippedDate DATETIME NULL,
    ShipVia INT NULL,
    Freight MONEY NULL DEFAULT(0),
    ShipName NVARCHAR(40) NULL,
    ShipAddress NVARCHAR(60) NULL,
    ShipCity NVARCHAR(15) NULL,
    ShipRegion NVARCHAR(15) NULL,
    ShipPostalCode NVARCHAR(10) NULL,
    ShipCountry NVARCHAR(15) NULL,

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION,

    -- Foreign Keys
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    CONSTRAINT FK_Orders_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    CONSTRAINT FK_Orders_Shippers FOREIGN KEY (ShipVia) REFERENCES Shippers(ShipperID)
);
GO

-- ==============================
-- Seed data for Orders
-- ==============================
INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry, created_by)
VALUES
(N'ALFKI', 1, '1996-07-04', '1996-08-01', '1996-07-16', 1, 32.38, N'Alfreds Futterkiste', N'Obere Str. 57', N'Berlin', NULL, N'12209', N'Germany', N'system'),
(N'ANATR', 3, '1996-07-05', '1996-08-16', '1996-07-10', 2, 11.61, N'Ana Trujillo Emparedados y helados', N'Avda. de la Constitución 2222', N'México D.F.', NULL, N'05021', N'Mexico', N'system'),
(N'ANTON', 4, '1996-07-08', '1996-08-05', '1996-07-12', 3, 45.34, N'Antonio Moreno Taquería', N'Mataderos 2312', N'México D.F.', NULL, N'05023', N'Mexico', N'system'),
(N'AROUT', 1, '1996-07-08', '1996-08-05', '1996-07-15', 1, 23.94, N'Around the Horn', N'120 Hanover Sq.', N'London', NULL, N'WA1 1DP', N'UK', N'system');
GO

-- ==============================
-- Table: Order Details
-- ==============================
CREATE TABLE OrderDetails (
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    UnitPrice MONEY NOT NULL DEFAULT(0),
    Quantity SMALLINT NOT NULL DEFAULT(1),
    Discount REAL NOT NULL DEFAULT(0),

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION,

    CONSTRAINT PK_OrderDetails PRIMARY KEY (OrderID, ProductID),
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

-- ==============================
-- Seed data for Order Details
-- ==============================
INSERT INTO OrderDetails (OrderID, ProductID, UnitPrice, Quantity, Discount, created_by)
VALUES
(1, 1, 18.00, 12, 0, N'system'),
(1, 2, 19.00, 10, 0, N'system'),
(2, 3, 10.00, 5, 0.1, N'system'),
(3, 4, 22.00, 20, 0, N'system'),
(3, 5, 21.35, 15, 0.05, N'system'),
(4, 6, 25.00, 30, 0, N'system'),
(4, 7, 30.00, 6, 0, N'system');
GO

/* ============================================================
   User Management
   ============================================================ */

-- ==============================
-- Table: User Roles
-- ==============================
CREATE TABLE UserRoles (
    UserRoleID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserRoleName NVARCHAR(50) NOT NULL,
    UserRoleDescription NVARCHAR(255) NULL,

    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION
);
GO

-- ==============================
-- Seed data for User Roles
-- ==============================
INSERT INTO UserRoles (UserRoleName, UserRoleDescription, created_by)
VALUES
(N'Administrator', N'Full system access', 'system'),
(N'Staff', N'Limited access for internal staff', 'system'),
(N'Customer', N'External customer account', 'system');
GO

-- ==============================
-- Table: Users
-- ==============================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(150) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    HashedPassword NVARCHAR(255) NOT NULL,
    UserRoleID INT NOT NULL,
    
    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION
    
    CONSTRAINT UQ_Users_Username UNIQUE(Username),
    CONSTRAINT UQ_Users_Email UNIQUE(Email),
    CONSTRAINT FK_Users_UserRoles FOREIGN KEY (UserRoleID) REFERENCES UserRoles(UserRoleID)
);
GO

-- UserTokens
CREATE TABLE UserTokens (
    UserTokenID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    AccessToken NVARCHAR(1024) NOT NULL,
    TokenType NVARCHAR(50) NOT NULL,
    DeviceType NVARCHAR(50) NOT NULL,
    RefreshToken NVARCHAR(1024) NOT NULL,
    
    -- BaseAuditable fields
    created_at DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    created_by NVARCHAR(50) NOT NULL,
    updated_at DATETIME NULL DEFAULT(GETUTCDATE()),
    updated_by NVARCHAR(50) NULL,
    row_version ROWVERSION
    
    CONSTRAINT FK_UserTokens_Users FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT UQ_User_Device UNIQUE (UserID, DeviceType)
);
GO