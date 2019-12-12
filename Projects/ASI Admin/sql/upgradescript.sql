 alter table USR_CatalogContactSale add ArtworkOption nvarchar(50) null;
 alter table USR_CatalogContactSale add ArtworkRepeatNotes nvarchar(500) null;
 alter table USR_CatalogContactSale ALTER COLUMN Email nvarchar (250);
 
 ALTER TABLE [dbo].[USR_CatalogArtWorks]
   ADD CONSTRAINT PK_USR_CatalogArtWorks PRIMARY KEY CLUSTERED ([ArtworkId]);
 GO
 
 ALTER TABLE [dbo].[USR_CatalogSaleArtworkMapping]  WITH CHECK ADD  CONSTRAINT [FK_USR_CatalogSaleArtworkMapping_USR_CatalogArtWorks] FOREIGN KEY([ArtworkId])
 REFERENCES [dbo].[USR_CatalogArtWorks] ([ArtworkId])
 GO

 ALTER TABLE [dbo].[USR_CatalogSaleArtworkMapping] CHECK CONSTRAINT [FK_USR_CatalogSaleArtworkMapping_USR_CatalogArtWorks]
 GO
 
ALTER TABLE USR_CatalogContactSale
ADD IsCancelled bit Not NUll DEFAULT 0;

ALTER TABLE USR_CatalogContactSale
ADD CancelledBy nvarchar(200) NULL;

ALTER TABLE USR_CatalogContactSale
ADD CancelledUTCDate datetime NULL;

ALTER TABLE USR_CatalogContact
ADD Note nvarchar(1000) NULL;

ALTER TABLE USR_CatalogContactSale
ADD RequestMoreInfo bit Not Null DEFAULT 0;
