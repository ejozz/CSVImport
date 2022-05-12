IF EXISTS (select 1 from sys.objects where object_id = object_id('SalesLT.ImportStaging'))
BEGIN
     DROP TABLE SalesLT.ImportStaging
END
	CREATE TABLE SalesLT.ImportStaging(
		Title VARCHAR(5),
		FirstName VARCHAR(25),
		MiddleName VARCHAR(25),
		LastName VARCHAR(25),
		Suffix VARCHAR(5),
		CompanyName VARCHAR(MAX),
		SalesPerson VARCHAR(MAX),
		EmailAddress VARCHAR(MAX),
		Phone VARCHAR(MAX),
		AddressLine1 VARCHAR(MAX),
		AddressLine2 VARCHAR(MAX),
		City VARCHAR(MAX),
		StateProvince VARCHAR(MAX),
		CountryRegion VARCHAR(MAX),
		PostalCode VARCHAR(MAX),
		AddressType VARCHAR(MAX),
		RevisionNumber VARCHAR(5),
		OrderDate VARCHAR(MAX),
		DueDate VARCHAR(MAX),
		ShipDate VARCHAR(MAX),
		Status VARCHAR(MAX),
		OnlineOrderFlag VARCHAR(MAX),
		SalesOrderNumber VARCHAR(MAX),
		PurchaseOrderNumber VARCHAR(MAX),
		AccountNumber VARCHAR(MAX),
		ShipMethod VARCHAR(MAX),
		CreditCardApprovalCode VARCHAR(MAX),
		SubTotal VARCHAR(MAX),
		TaxAmt VARCHAR(MAX),
		Freight VARCHAR(MAX),
		TotalDue VARCHAR(MAX),
		Comment VARCHAR(MAX),
		OrderQty VARCHAR(MAX),
		ProductID VARCHAR(MAX),
		UnitPrice VARCHAR(MAX),
		UnitPriceDiscount VARCHAR(MAX),
		LineTotal VARCHAR(MAX)
	)

GO

IF EXISTS (select 1 from sys.objects where object_id = object_id('SalesLT.ProcessImport'))
BEGIN
     DROP PROC SalesLT.ProcessImport
END
GO
CREATE PROC SalesLT.ProcessImport AS 

INSERT INTO SalesLT.Customer(Title, FirstName, MiddleName, LastName,Suffix, CompanyName,SalesPerson,EmailAddress,Phone, ModifiedDate)
SELECT DISTINCT st.Title, st.FirstName, st.MiddleName, st.LastName, st.Suffix, st.CompanyName, st.SalesPerson, st.EmailAddress, st.Phone, GETDATE() 
FROM SalesLT.ImportStaging st
LEFT JOIN SalesLT.Customer c ON  st.FirstName = c.FirstName AND st.LastName = c.LastName
WHERE c.CustomerId IS NULL

INSERT INTO SalesLT.Address(AddressLine1, AddressLine2, City, StateProvince, CountryRegion, PostalCode, ModifiedDate)
SELECT DISTINCT st.AddressLine1, st.AddressLine2, st.City, st.StateProvince, st.CountryRegion, st.PostalCode, GETDATE() 
FROM SalesLT.ImportStaging st
LEFT JOIN SalesLT.Address c ON  st.AddressLine1 = c.AddressLine1 AND st.PostalCode = c.PostalCode
WHERE c.AddressID IS NULL

INSERT INTO SalesLT.CustomerAddress
SELECT DISTINCT c.CustomerID, a.AddressID, st.AddressType, GETDATE() 
FROM SalesLT.ImportStaging st 
INNER JOIN SalesLT.Customer c ON st.FirstName = c.FirstName AND st.LastName = c.LastName
INNER JOIN SalesLT.Address a ON st.AddressLine1 = a.AddressLine1
LEFT JOIN SalesLT.CustomerAddress ca ON ca.CustomerID = c.CustomerID AND ca.AddressID = a.AddressID
WHERE ca.CustomerID IS NULL

INSERT INTO SalesLT.SalesOrderHeader(
	RevisionNumber, OrderDate, DueDate, 
	ShipDate, Status, OnlineOrderFlag, PurchaseOrderNumber, 
	AccountNumber, CustomerID, ShipToAddressID, BillToAddressID, 
	ShipMethod, CreditCardApprovalCode, SubTotal, TaxAmt, Freight,
	Comment, ModifiedDate
)
SELECT DISTINCT 
	RevisionNumber, OrderDate, DueDate, 
	ShipDate, Status, OnlineOrderFlag, PurchaseOrderNumber, 
	AccountNumber, c.CustomerID, AddressID, AddressID, 
	ShipMethod, CreditCardApprovalCode, SubTotal, TaxAmt, Freight,
	Comment, GETDATE()
FROM 
	SalesLT.ImportStaging st 
	INNER JOIN SalesLT.Customer c ON st.FirstName = c.FirstName AND st.LastName = c.LastName
	INNER JOIN SalesLT.CustomerAddress ca ON c.CustomerID = ca.CustomerID


INSERT INTO SalesLT.SalesOrderDetail(SalesOrderID, OrderQty, ProductID, UnitPrice, UnitPriceDiscount, ModifiedDate)
SELECT SalesOrderID, OrderQty, ProductID, UnitPrice, UnitPriceDiscount, GETDATE()
FROM SalesLT.ImportStaging st
	INNER JOIN SalesLT.Customer c ON st.FirstName = c.FirstName AND st.LastName = c.LastName
	INNER JOIN SalesLT.SalesOrderHeader soh ON soh.CustomerID = c.CustomerID


DELETE FROM SalesLT.ImportStaging


GO