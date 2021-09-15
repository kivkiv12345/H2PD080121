-- MySQL dump 10.13  Distrib 8.0.26, for Linux (x86_64)
--
-- Host: localhost    Database: schema_H2
-- ------------------------------------------------------
-- Server version	8.0.26-0ubuntu0.20.04.2

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `ContactInformation`
--

CREATE DATABASE IF NOT EXISTS schema_H2;

DROP TABLE IF EXISTS `ContactInformation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ContactInformation` (
  `cprNumber` bigint unsigned NOT NULL,
  `phoneNumber` int DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`cprNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ContactInformation`
--

LOCK TABLES `ContactInformation` WRITE;
/*!40000 ALTER TABLE `ContactInformation` DISABLE KEYS */;
INSERT INTO `ContactInformation` VALUES (1234567890,NULL,NULL),(3328142926,NULL,NULL),(5772618857,NULL,NULL),(6180741191,NULL,NULL),(7322858337,NULL,NULL),(7629863185,NULL,NULL),(8178317211,NULL,NULL);
/*!40000 ALTER TABLE `ContactInformation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Grade`
--

DROP TABLE IF EXISTS `Grade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Grade` (
  `student` bigint unsigned NOT NULL,
  `subject` bigint unsigned NOT NULL,
  `date` date NOT NULL,
  `grade` enum('12','10','7','4','2','02') DEFAULT NULL,
  PRIMARY KEY (`student`,`subject`),
  KEY `fk_Grade_1_idx` (`subject`),
  CONSTRAINT `fk_Grade_1` FOREIGN KEY (`subject`) REFERENCES `Subject` (`subjectid`),
  CONSTRAINT `fk_Grade_2` FOREIGN KEY (`student`) REFERENCES `Student` (`person`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Grade`
--

LOCK TABLES `Grade` WRITE;
/*!40000 ALTER TABLE `Grade` DISABLE KEYS */;
INSERT INTO `Grade` VALUES (7,3,'2021-08-27','12'),(8,3,'2021-08-27','10');
/*!40000 ALTER TABLE `Grade` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `LogCreatePerson`
--

DROP TABLE IF EXISTS `LogCreatePerson`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LogCreatePerson` (
  `LogCreateId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `message` varchar(75) NOT NULL,
  PRIMARY KEY (`LogCreateId`),
  UNIQUE KEY `LogCreateId_UNIQUE` (`LogCreateId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `LogCreatePerson`
--

LOCK TABLES `LogCreatePerson` WRITE;
/*!40000 ALTER TABLE `LogCreatePerson` DISABLE KEYS */;
INSERT INTO `LogCreatePerson` VALUES (1,'4 Egon 2021-08-27 11:46:53'),(2,'5 Kaj 2021-08-27 11:50:26'),(3,'6 Gabe 2021-08-27 11:50:46'),(4,'7 William 2021-08-27 11:50:58'),(5,'8 Jens 2021-08-27 11:51:13'),(6,'9 Karl 2021-08-27 11:54:01'),(7,'10 Dudebro 2021-08-27 12:02:46');
/*!40000 ALTER TABLE `LogCreatePerson` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Parent`
--

DROP TABLE IF EXISTS `Parent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Parent` (
  `person` bigint unsigned NOT NULL,
  PRIMARY KEY (`person`),
  UNIQUE KEY `person` (`person`),
  CONSTRAINT `fk_Parent_1` FOREIGN KEY (`person`) REFERENCES `Person` (`personId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Parent`
--

LOCK TABLES `Parent` WRITE;
/*!40000 ALTER TABLE `Parent` DISABLE KEYS */;
INSERT INTO `Parent` VALUES (4),(5);
/*!40000 ALTER TABLE `Parent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Parent_X_Student`
--

DROP TABLE IF EXISTS `Parent_X_Student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Parent_X_Student` (
  `parent` bigint unsigned NOT NULL,
  `student` bigint unsigned NOT NULL,
  PRIMARY KEY (`parent`,`student`),
  KEY `Parent_X_Student_FK` (`student`),
  CONSTRAINT `Parent_X_Student_FK` FOREIGN KEY (`student`) REFERENCES `Student` (`person`),
  CONSTRAINT `Parent_X_Student_FK_1` FOREIGN KEY (`parent`) REFERENCES `Parent` (`person`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Parent_X_Student`
--

LOCK TABLES `Parent_X_Student` WRITE;
/*!40000 ALTER TABLE `Parent_X_Student` DISABLE KEYS */;
/*!40000 ALTER TABLE `Parent_X_Student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Person`
--

DROP TABLE IF EXISTS `Person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Person` (
  `personId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `contactInformation` bigint unsigned DEFAULT NULL,
  PRIMARY KEY (`personId`),
  KEY `fk_Person_1_idx` (`contactInformation`),
  CONSTRAINT `fk_Person_1` FOREIGN KEY (`contactInformation`) REFERENCES `ContactInformation` (`cprNumber`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Person`
--

LOCK TABLES `Person` WRITE;
/*!40000 ALTER TABLE `Person` DISABLE KEYS */;
INSERT INTO `Person` VALUES (4,'Egon',1234567890),(5,'Kaj',8178317211),(6,'Gabe',5772618857),(7,'William',3328142926),(8,'Jens',7322858337),(9,'Karl',7629863185),(10,'Dudebro',6180741191);
/*!40000 ALTER TABLE `Person` ENABLE KEYS */;
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
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `triggerPersonLog` AFTER INSERT ON `Person` FOR EACH ROW INSERT INTO LogCreatePerson(message) VALUES (CONCAT(NEW.personId, " ", NEW.name, " ", NOW())) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `Person_X_Team`
--

DROP TABLE IF EXISTS `Person_X_Team`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Person_X_Team` (
  `person` bigint unsigned NOT NULL,
  `team` varchar(25) NOT NULL,
  PRIMARY KEY (`person`,`team`),
  KEY `fk_Person_X_Team_2` (`team`),
  CONSTRAINT `fk_Person_X_Team_1` FOREIGN KEY (`person`) REFERENCES `Person` (`personId`),
  CONSTRAINT `fk_Person_X_Team_2` FOREIGN KEY (`team`) REFERENCES `Team` (`teamName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Person_X_Team`
--

LOCK TABLES `Person_X_Team` WRITE;
/*!40000 ALTER TABLE `Person_X_Team` DISABLE KEYS */;
/*!40000 ALTER TABLE `Person_X_Team` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Student`
--

DROP TABLE IF EXISTS `Student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Student` (
  `person` bigint unsigned NOT NULL,
  `team` varchar(25) NOT NULL,
  PRIMARY KEY (`person`),
  UNIQUE KEY `person` (`person`),
  KEY `Student_FK_1` (`team`),
  CONSTRAINT `fk_Student_1` FOREIGN KEY (`person`) REFERENCES `Person` (`personId`),
  CONSTRAINT `Student_FK_1` FOREIGN KEY (`team`) REFERENCES `Team` (`teamName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Student`
--

LOCK TABLES `Student` WRITE;
/*!40000 ALTER TABLE `Student` DISABLE KEYS */;
INSERT INTO `Student` VALUES (7,'H1111111'),(8,'H2222222');
/*!40000 ALTER TABLE `Student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `StudentsOfClasses`
--

DROP TABLE IF EXISTS `StudentsOfClasses`;
/*!50001 DROP VIEW IF EXISTS `StudentsOfClasses`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `StudentsOfClasses` AS SELECT 
 1 AS `name`,
 1 AS `team`,
 1 AS `phoneNumber`,
 1 AS `email`,
 1 AS `cprNumber`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `Subject`
--

DROP TABLE IF EXISTS `Subject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Subject` (
  `subjectid` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`subjectid`),
  UNIQUE KEY `idSubject_UNIQUE` (`subjectid`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Subject`
--

LOCK TABLES `Subject` WRITE;
/*!40000 ALTER TABLE `Subject` DISABLE KEYS */;
INSERT INTO `Subject` VALUES (3,'Database Programming'),(1,'English'),(2,'Seriousness');
/*!40000 ALTER TABLE `Subject` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SubjectModule`
--

DROP TABLE IF EXISTS `SubjectModule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SubjectModule` (
  `team` varchar(25) NOT NULL,
  `startTime` datetime NOT NULL,
  `endDate` datetime NOT NULL,
  `teacher` bigint unsigned NOT NULL,
  `subject` bigint unsigned NOT NULL,
  PRIMARY KEY (`team`),
  UNIQUE KEY `team_UNIQUE` (`team`),
  KEY `fk_SubjectModule_1_idx` (`teacher`),
  KEY `fk_SubjectModule_3_idx` (`subject`),
  CONSTRAINT `fk_SubjectModule_1` FOREIGN KEY (`teacher`) REFERENCES `Teacher` (`person`),
  CONSTRAINT `fk_SubjectModule_2` FOREIGN KEY (`team`) REFERENCES `Team` (`teamName`),
  CONSTRAINT `fk_SubjectModule_3` FOREIGN KEY (`subject`) REFERENCES `Subject` (`subjectid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SubjectModule`
--

LOCK TABLES `SubjectModule` WRITE;
/*!40000 ALTER TABLE `SubjectModule` DISABLE KEYS */;
/*!40000 ALTER TABLE `SubjectModule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SubjectTeacher`
--

DROP TABLE IF EXISTS `SubjectTeacher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SubjectTeacher` (
  `teacher` bigint unsigned NOT NULL,
  `subject` bigint unsigned NOT NULL,
  PRIMARY KEY (`teacher`,`subject`),
  KEY `fk_SubjectTeacher_2_idx` (`subject`),
  CONSTRAINT `fk_SubjectTeacher_1` FOREIGN KEY (`teacher`) REFERENCES `Teacher` (`person`),
  CONSTRAINT `fk_SubjectTeacher_2` FOREIGN KEY (`subject`) REFERENCES `Subject` (`subjectid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SubjectTeacher`
--

LOCK TABLES `SubjectTeacher` WRITE;
/*!40000 ALTER TABLE `SubjectTeacher` DISABLE KEYS */;
INSERT INTO `SubjectTeacher` VALUES (5,1),(6,3);
/*!40000 ALTER TABLE `SubjectTeacher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Teacher`
--

DROP TABLE IF EXISTS `Teacher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Teacher` (
  `person` bigint unsigned NOT NULL,
  PRIMARY KEY (`person`),
  UNIQUE KEY `person` (`person`),
  CONSTRAINT `fk_Teacher_1` FOREIGN KEY (`person`) REFERENCES `Person` (`personId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Teacher`
--

LOCK TABLES `Teacher` WRITE;
/*!40000 ALTER TABLE `Teacher` DISABLE KEYS */;
INSERT INTO `Teacher` VALUES (5),(6);
/*!40000 ALTER TABLE `Teacher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Team`
--

DROP TABLE IF EXISTS `Team`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Team` (
  `teamName` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`teamName`),
  UNIQUE KEY `teamName_UNIQUE` (`teamName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Team`
--

LOCK TABLES `Team` WRITE;
/*!40000 ALTER TABLE `Team` DISABLE KEYS */;
INSERT INTO `Team` VALUES ('H1111111'),('H2222222'),('H3333333'),('H4444444');
/*!40000 ALTER TABLE `Team` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Final view structure for view `StudentsOfClasses`
--

/*!50001 DROP VIEW IF EXISTS `StudentsOfClasses`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `StudentsOfClasses` AS select `Person`.`name` AS `name`,`Student`.`team` AS `team`,`ContactInformation`.`phoneNumber` AS `phoneNumber`,`ContactInformation`.`email` AS `email`,`ContactInformation`.`cprNumber` AS `cprNumber` from ((`Student` left join `Person` on((`Student`.`person` = `Person`.`personId`))) left join `ContactInformation` on((`Person`.`contactInformation` = `ContactInformation`.`cprNumber`))) */;
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

-- Dump completed on 2021-09-15 10:48:45
