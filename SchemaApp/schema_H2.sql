-- Dump completed on 2021-09-21  5:50:08
CREATE DATABASE  IF NOT EXISTS `schema_h2` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `schema_h2`;
-- MySQL dump 10.13  Distrib 8.0.26, for Win64 (x86_64)
--
-- Host: localhost    Database: schema_h2
-- ------------------------------------------------------
-- Server version	8.0.26

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `contactinformation`
--

DROP TABLE IF EXISTS `contactinformation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contactinformation` (
  `cprNumber` bigint unsigned NOT NULL,
  `phoneNumber` int DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`cprNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contactinformation`
--

LOCK TABLES `contactinformation` WRITE;
/*!40000 ALTER TABLE `contactinformation` DISABLE KEYS */;
INSERT INTO `contactinformation` VALUES (1234567890,NULL,NULL),(3328142926,NULL,NULL),(5772618857,NULL,NULL),(6180741191,NULL,NULL),(7322858337,NULL,NULL),(7629863185,NULL,NULL),(8178317211,NULL,NULL);
/*!40000 ALTER TABLE `contactinformation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `grade`
--

DROP TABLE IF EXISTS `grade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `grade` (
  `student` bigint unsigned NOT NULL,
  `subject` bigint unsigned NOT NULL,
  `date` date NOT NULL,
  `grade` enum('12','10','7','4','2','02') DEFAULT NULL,
  PRIMARY KEY (`student`,`subject`),
  KEY `fk_Grade_1_idx` (`subject`),
  CONSTRAINT `fk_Grade_1` FOREIGN KEY (`subject`) REFERENCES `subject` (`subjectid`),
  CONSTRAINT `fk_Grade_2` FOREIGN KEY (`student`) REFERENCES `student` (`person`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `grade`
--

LOCK TABLES `grade` WRITE;
/*!40000 ALTER TABLE `grade` DISABLE KEYS */;
INSERT INTO `grade` VALUES (7,3,'2021-08-27','12'),(8,3,'2021-08-27','10');
/*!40000 ALTER TABLE `grade` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `logcreateperson`
--

DROP TABLE IF EXISTS `logcreateperson`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `logcreateperson` (
  `LogCreateId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `message` varchar(75) NOT NULL,
  PRIMARY KEY (`LogCreateId`),
  UNIQUE KEY `LogCreateId_UNIQUE` (`LogCreateId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `logcreateperson`
--

LOCK TABLES `logcreateperson` WRITE;
/*!40000 ALTER TABLE `logcreateperson` DISABLE KEYS */;
INSERT INTO `logcreateperson` VALUES (1,'4 Egon 2021-08-27 11:46:53'),(2,'5 Kaj 2021-08-27 11:50:26'),(3,'6 Gabe 2021-08-27 11:50:46'),(4,'7 William 2021-08-27 11:50:58'),(5,'8 Jens 2021-08-27 11:51:13'),(6,'9 Karl 2021-08-27 11:54:01'),(7,'10 Dudebro 2021-08-27 12:02:46');
/*!40000 ALTER TABLE `logcreateperson` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `parent`
--

DROP TABLE IF EXISTS `parent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `parent` (
  `person` bigint unsigned NOT NULL,
  PRIMARY KEY (`person`),
  UNIQUE KEY `person` (`person`),
  CONSTRAINT `fk_Parent_1` FOREIGN KEY (`person`) REFERENCES `person` (`personId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parent`
--

LOCK TABLES `parent` WRITE;
/*!40000 ALTER TABLE `parent` DISABLE KEYS */;
INSERT INTO `parent` VALUES (4),(5);
/*!40000 ALTER TABLE `parent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `parent_x_student`
--

DROP TABLE IF EXISTS `parent_x_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `parent_x_student` (
  `parent` bigint unsigned NOT NULL,
  `student` bigint unsigned NOT NULL,
  PRIMARY KEY (`parent`,`student`),
  KEY `Parent_X_Student_FK` (`student`),
  CONSTRAINT `Parent_X_Student_FK` FOREIGN KEY (`student`) REFERENCES `student` (`person`),
  CONSTRAINT `Parent_X_Student_FK_1` FOREIGN KEY (`parent`) REFERENCES `parent` (`person`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parent_x_student`
--

LOCK TABLES `parent_x_student` WRITE;
/*!40000 ALTER TABLE `parent_x_student` DISABLE KEYS */;
/*!40000 ALTER TABLE `parent_x_student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `person`
--

DROP TABLE IF EXISTS `person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `person` (
  `personId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `contactInformation` bigint unsigned DEFAULT NULL,
  `Birthday` date DEFAULT NULL,
  `Sex` enum('MALE','FEMALE') NOT NULL,
  PRIMARY KEY (`personId`),
  KEY `fk_Person_1_idx` (`contactInformation`),
  CONSTRAINT `fk_Person_1` FOREIGN KEY (`contactInformation`) REFERENCES `contactinformation` (`cprNumber`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `person`
--

LOCK TABLES `person` WRITE;
/*!40000 ALTER TABLE `person` DISABLE KEYS */;
INSERT INTO `person` VALUES (4,'Egon','0',1234567890,NULL,'MALE'),(5,'Kaj','0',8178317211,NULL,'MALE'),(6,'Gabe','0',5772618857,NULL,'MALE'),(7,'William','0',3328142926,NULL,'MALE'),(8,'Jens','0',7322858337,NULL,'MALE'),(9,'Karl','0',7629863185,NULL,'MALE'),(10,'Dudebro','0',6180741191,NULL,'MALE');
/*!40000 ALTER TABLE `person` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `triggerPersonLog` AFTER INSERT ON `person` FOR EACH ROW INSERT INTO LogCreatePerson(message) VALUES (CONCAT(NEW.personId, " ", NEW.FirstName, " ", NOW())) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `person_x_team`
--

DROP TABLE IF EXISTS `person_x_team`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `person_x_team` (
  `person` bigint unsigned NOT NULL,
  `team` varchar(25) NOT NULL,
  PRIMARY KEY (`person`,`team`),
  KEY `fk_Person_X_Team_2` (`team`),
  CONSTRAINT `fk_Person_X_Team_1` FOREIGN KEY (`person`) REFERENCES `person` (`personId`),
  CONSTRAINT `fk_Person_X_Team_2` FOREIGN KEY (`team`) REFERENCES `team` (`teamName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `person_x_team`
--

LOCK TABLES `person_x_team` WRITE;
/*!40000 ALTER TABLE `person_x_team` DISABLE KEYS */;
/*!40000 ALTER TABLE `person_x_team` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student` (
  `person` bigint unsigned NOT NULL,
  `team` varchar(25) NOT NULL,
  PRIMARY KEY (`person`),
  UNIQUE KEY `person` (`person`),
  KEY `Student_FK_1` (`team`),
  CONSTRAINT `fk_Student_1` FOREIGN KEY (`person`) REFERENCES `person` (`personId`),
  CONSTRAINT `Student_FK_1` FOREIGN KEY (`team`) REFERENCES `team` (`teamName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
INSERT INTO `student` VALUES (7,'H1111111'),(8,'H2222222');
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `studentsofclasses`
--

DROP TABLE IF EXISTS `studentsofclasses`;
/*!50001 DROP VIEW IF EXISTS `studentsofclasses`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `studentsofclasses` AS SELECT 
 1 AS `FirstName`,
 1 AS `LastName`,
 1 AS `team`,
 1 AS `phoneNumber`,
 1 AS `email`,
 1 AS `cprNumber`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `subject`
--

DROP TABLE IF EXISTS `subject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subject` (
  `subjectid` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`subjectid`),
  UNIQUE KEY `idSubject_UNIQUE` (`subjectid`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subject`
--

LOCK TABLES `subject` WRITE;
/*!40000 ALTER TABLE `subject` DISABLE KEYS */;
INSERT INTO `subject` VALUES (3,'Database Programming'),(1,'English'),(2,'Seriousness');
/*!40000 ALTER TABLE `subject` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subjectmodule`
--

DROP TABLE IF EXISTS `subjectmodule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subjectmodule` (
  `team` varchar(25) NOT NULL,
  `startTime` datetime NOT NULL,
  `endDate` datetime NOT NULL,
  `teacher` bigint unsigned NOT NULL,
  `subject` bigint unsigned NOT NULL,
  PRIMARY KEY (`team`),
  UNIQUE KEY `team_UNIQUE` (`team`),
  KEY `fk_SubjectModule_1_idx` (`teacher`),
  KEY `fk_SubjectModule_3_idx` (`subject`),
  CONSTRAINT `fk_SubjectModule_1` FOREIGN KEY (`teacher`) REFERENCES `teacher` (`person`),
  CONSTRAINT `fk_SubjectModule_2` FOREIGN KEY (`team`) REFERENCES `team` (`teamName`),
  CONSTRAINT `fk_SubjectModule_3` FOREIGN KEY (`subject`) REFERENCES `subject` (`subjectid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subjectmodule`
--

LOCK TABLES `subjectmodule` WRITE;
/*!40000 ALTER TABLE `subjectmodule` DISABLE KEYS */;
/*!40000 ALTER TABLE `subjectmodule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subjectteacher`
--

DROP TABLE IF EXISTS `subjectteacher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subjectteacher` (
  `teacher` bigint unsigned NOT NULL,
  `subject` bigint unsigned NOT NULL,
  PRIMARY KEY (`teacher`,`subject`),
  KEY `fk_SubjectTeacher_2_idx` (`subject`),
  CONSTRAINT `fk_SubjectTeacher_1` FOREIGN KEY (`teacher`) REFERENCES `teacher` (`person`),
  CONSTRAINT `fk_SubjectTeacher_2` FOREIGN KEY (`subject`) REFERENCES `subject` (`subjectid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subjectteacher`
--

LOCK TABLES `subjectteacher` WRITE;
/*!40000 ALTER TABLE `subjectteacher` DISABLE KEYS */;
INSERT INTO `subjectteacher` VALUES (5,1),(6,3);
/*!40000 ALTER TABLE `subjectteacher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `teacher`
--

DROP TABLE IF EXISTS `teacher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `teacher` (
  `person` bigint unsigned NOT NULL,
  PRIMARY KEY (`person`),
  UNIQUE KEY `person` (`person`),
  CONSTRAINT `fk_Teacher_1` FOREIGN KEY (`person`) REFERENCES `person` (`personId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `teacher`
--

LOCK TABLES `teacher` WRITE;
/*!40000 ALTER TABLE `teacher` DISABLE KEYS */;
INSERT INTO `teacher` VALUES (5),(6);
/*!40000 ALTER TABLE `teacher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `team`
--

DROP TABLE IF EXISTS `team`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `team` (
  `teamName` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`teamName`),
  UNIQUE KEY `teamName_UNIQUE` (`teamName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `team`
--

LOCK TABLES `team` WRITE;
/*!40000 ALTER TABLE `team` DISABLE KEYS */;
INSERT INTO `team` VALUES ('H1111111'),('H2222222'),('H3333333'),('H4444444');
/*!40000 ALTER TABLE `team` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'schema_h2'
--

--
-- Dumping routines for database 'schema_h2'
--

--
-- Final view structure for view `studentsofclasses`
--

/*!50001 DROP VIEW IF EXISTS `studentsofclasses`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `studentsofclasses` AS select `person`.`FirstName` AS `FirstName`,`person`.`LastName` AS `LastName`,`student`.`team` AS `team`,`contactinformation`.`phoneNumber` AS `phoneNumber`,`contactinformation`.`email` AS `email`,`contactinformation`.`cprNumber` AS `cprNumber` from ((`student` left join `person` on((`student`.`person` = `person`.`personId`))) left join `contactinformation` on((`person`.`contactInformation` = `contactinformation`.`cprNumber`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-09-21  5:50:08