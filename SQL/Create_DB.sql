USE Master

IF EXISTS(SELECT * FROM sysdatabases WHERE name = 'SWE_Temperatur')
  DROP DATABASE SWE_Temperatur
GO

CREATE DATABASE SWE_Temperatur

ON PRIMARY (
  Name = 'SWE_Temperatur_DATA',
  Filename='C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\SWE_Temperatur_DATA.mdf',
  Size = 10MB,
  Filegrowth = 10%
)
LOG ON (
  Name = 'SWE_Temperatur_Log',
  Filename='C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\SWE_Temperatur_LOG.ldf',
  Size = 10MB,
  Filegrowth = 10%
)
