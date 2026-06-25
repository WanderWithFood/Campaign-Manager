-- ============================================================
-- Campaign Management Database Schema
-- MySQL 8.0+
-- ============================================================

-- Create Database
CREATE DATABASE IF NOT EXISTS `campaignmanagement` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `campaignmanagement`;

-- ============================================================
-- Table: mstAccessLevel
-- ============================================================
CREATE TABLE IF NOT EXISTS `mstAccessLevel` (
    `mstAccessLevelId` INT NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(100) NOT NULL,
    `isActive` TINYINT(1) NOT NULL DEFAULT 1,
    `createdBy` INT NULL,
    `created_at` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedBy` INT NULL,
    `updated_at` DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`mstAccessLevelId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Table: mstUsers
-- ============================================================
CREATE TABLE IF NOT EXISTS `mstUsers` (
    `mstUserId` INT NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    `gender` VARCHAR(20) NULL,
    `phoneNumber` VARCHAR(20) NULL,
    `email` VARCHAR(255) NOT NULL,
    `password` VARCHAR(255) NULL,
    `dateOfBirth` DATETIME NULL,
    `FirebaseId` VARCHAR(255) NULL,
    `userAccessLevel` INT NOT NULL DEFAULT 1,
    `isActive` TINYINT(1) NOT NULL DEFAULT 1,
    `createdBy` INT NULL,
    `created_at` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedBy` INT NULL,
    `updated_at` DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    `risk_score` INT NOT NULL DEFAULT 0,
    `streak_count` INT NOT NULL DEFAULT 0,
    `longest_streak` INT NOT NULL DEFAULT 0,
    `last_active_at` DATETIME NULL,
    `user_status` VARCHAR(50) NULL,
    `user_tags` VARCHAR(500) NULL,
    `is_shadow_banned` TINYINT(1) NOT NULL DEFAULT 0,
    `notes` TEXT NULL,
    `device_type` VARCHAR(50) NULL,
    `app_version` VARCHAR(50) NULL,
    PRIMARY KEY (`mstUserId`),
    INDEX `idx_email` (`email`),
    INDEX `idx_accessLevel` (`userAccessLevel`),
    CONSTRAINT `fk_user_accessLevel` FOREIGN KEY (`userAccessLevel`) REFERENCES `mstAccessLevel`(`mstAccessLevelId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Table: mstCoreAdmin
-- ============================================================
CREATE TABLE IF NOT EXISTS `mstCoreAdmin` (
    `mstCoreAdminId` INT NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    `email` VARCHAR(255) NOT NULL,
    `password` VARCHAR(255) NULL,
    `isActive` TINYINT(1) NOT NULL DEFAULT 1,
    `created_at` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`mstCoreAdminId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Table: trnUserActivity
-- ============================================================
CREATE TABLE IF NOT EXISTS `trnUserActivity` (
    `trnUserActivityId` INT NOT NULL AUTO_INCREMENT,
    `userId` INT NOT NULL,
    `activityDateTime` DATETIME NOT NULL,
    `requestMethod` VARCHAR(10) NOT NULL,
    `queryParams` VARCHAR(500) NULL,
    `IPAddress` VARCHAR(50) NULL,
    `pageUrl` VARCHAR(500) NULL,
    `remarks` TEXT NULL,
    `functionName` VARCHAR(255) NULL,
    PRIMARY KEY (`trnUserActivityId`),
    INDEX `idx_userId` (`userId`),
    INDEX `idx_activityDate` (`activityDateTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Table: trnUserOtps
-- ============================================================
CREATE TABLE IF NOT EXISTS `trnUserOtps` (
    `trnUserOtpId` INT NOT NULL AUTO_INCREMENT,
    `userId` INT NOT NULL,
    `otp` VARCHAR(10) NOT NULL,
    `createdAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `expiresAt` DATETIME NOT NULL,
    `isUsed` TINYINT(1) NOT NULL DEFAULT 0,
    PRIMARY KEY (`trnUserOtpId`),
    INDEX `idx_userId_otp` (`userId`, `otp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Seed Data: Default Access Levels
-- ============================================================
INSERT INTO `mstAccessLevel` (`name`, `isActive`, `created_at`) VALUES
('User', 1, NOW()),
('Admin', 1, NOW()),
('Super Admin', 1, NOW());

-- ============================================================
-- Seed Data: Default Admin User (password: Admin@123)
-- ============================================================
INSERT INTO `mstUsers` (`name`, `email`, `password`, `userAccessLevel`, `isActive`, `created_at`) VALUES
('Admin', 'admin@campaignmanagement.com', 'Admin@123', 3, 1, NOW());
