USE [master]

DECLARE @DBNAME VARCHAR(MAX)
DECLARE @DataPath AS NVARCHAR(MAX)
DECLARE @sql VARCHAR(MAX)

SET @DBNAME = N'SampleDB'
SET @DataPath = N'/var/opt/mssql/data/'

SELECT @sql = 'USE MASTER'
EXEC (@sql)

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @DBNAME OR name = @DBNAME)))
BEGIN
	SELECT @sql = 'DROP DATABASE ' + quotename(@DBNAME)
	EXEC (@sql)
END

SELECT @sql = 'CREATE DATABASE ' + quotename(@DBNAME) + ' 
ON 
PRIMARY
( 
NAME = ''' + @DBNAME + '_DB'', 
FILENAME = ''' + @DataPath + '\DB\' + @DBNAME + '.mdf'', 
SIZE = 3136 KB , MAXSIZE = UNLIMITED, 
FILEGROWTH = 1024 KB
) 
LOG ON
(
NAME = ''' + @DBNAME + '_Log'', 
FILENAME = ''' + @DataPath + '\Logs\' + @DBNAME  + '_log.ldf'', 
SIZE = 832KB , MAXSIZE =  2048 GB , FILEGROWTH = 10 %
)'

EXEC (@sql)