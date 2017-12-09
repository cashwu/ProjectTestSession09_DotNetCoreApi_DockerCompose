USE [SampleDB]

CREATE TABLE dbo.NewTaipeiWifiSpot 
(
    Id             NVARCHAR(50) NOT NULL ,
    Spot_Name      NVARCHAR(50) NOT NULL ,
    Type           NVARCHAR(50) NOT NULL ,
    Company        NVARCHAR(50) NOT NULL ,
    District       NVARCHAR(10) NOT NULL ,
    Address        NVARCHAR(200) NOT NULL ,
    Apparatus_Name NVARCHAR(50) NOT NULL ,
    Latitude       NVARCHAR(50) NOT NULL ,
    Longitude      NVARCHAR(50) NOT NULL ,
    TWD97X         NVARCHAR(50) NOT NULL ,
    TWD97Y         NVARCHAR(50) NOT NULL ,
    WGS84aX        NVARCHAR(50) NOT NULL ,
    WGS84aY        NVARCHAR(50) NOT NULL ,
    CONSTRAINT PK_NewTaipeiWifiSpot PRIMARY KEY CLUSTERED(Id ASC)
)