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

-- ============================================================
-- Table: mstCampaigns
-- ============================================================
CREATE TABLE IF NOT EXISTS `mstCampaigns` (
    `mstCampaignId` INT NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    `campaignType` VARCHAR(100) NULL,
    `campaignCycle` VARCHAR(100) NULL,
    `startDate` DATETIME NULL,
    `endDate` DATETIME NULL,
    `duration` INT NULL,
    `totalSpend` DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    `conversionRate` DECIMAL(5, 2) NOT NULL DEFAULT 0.00,
    `downloadsBefore` INT NOT NULL DEFAULT 0,
    `downloadsAfter` INT NOT NULL DEFAULT 0,
    `objective` TEXT NULL,
    `targetAudience` VARCHAR(255) NULL,
    `budget` DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    `creatorName` VARCHAR(255) NULL,
    `socialMediaPlatforms` VARCHAR(255) NULL,
    `status` VARCHAR(50) NOT NULL DEFAULT 'Active',
    `isActive` TINYINT(1) NOT NULL DEFAULT 1,
    `createdBy` INT NULL,
    `created_at` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedBy` INT NULL,
    `updated_at` DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`mstCampaignId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Table: mstInfluencers
-- ============================================================
CREATE TABLE IF NOT EXISTS `mstInfluencers` (
    `mstInfluencerId` INT NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    `category` VARCHAR(255) NULL,
    `location` VARCHAR(100) NULL,
    `dateOfBirth` DATETIME NULL,
    `gender` VARCHAR(20) NULL,
    `socialMediaPlatforms` VARCHAR(255) NULL,
    `creatorId` VARCHAR(50) NULL,
    `managerName` VARCHAR(100) NULL,
    `managerNumber` VARCHAR(20) NULL,
    `instagramProfile` VARCHAR(255) NULL,
    `whatsAppContact` VARCHAR(255) NULL,
    `integrationRequirements` TEXT NULL,
    `exclusivityClause` TEXT NULL,
    `contentDurationClause` TEXT NULL,
    `paymentTerms` TEXT NULL,
    `followers` VARCHAR(50) NULL,
    `engagement` DECIMAL(5, 2) NOT NULL DEFAULT 0.00,
    `avgViews` VARCHAR(50) NULL,
    `estConversion` INT NOT NULL DEFAULT 0,
    `estCostMin` DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    `estCostMax` DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    `reliabilityScore` INT NOT NULL DEFAULT 0,
    `notes` TEXT NULL,
    `isActive` TINYINT(1) NOT NULL DEFAULT 1,
    `created_at` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`mstInfluencerId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Table: trnExpenses
-- ============================================================
CREATE TABLE IF NOT EXISTS `trnExpenses` (
    `trnExpenseId` INT NOT NULL AUTO_INCREMENT,
    `campaignId` INT NOT NULL,
    `expenseName` VARCHAR(255) NOT NULL,
    `category` VARCHAR(100) NULL,
    `paidTo` VARCHAR(255) NULL,
    `amount` DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    `status` VARCHAR(50) NOT NULL DEFAULT 'Pending',
    `expenseDate` DATETIME NULL,
    `proofPath` VARCHAR(500) NULL,
    `isActive` TINYINT(1) NOT NULL DEFAULT 1,
    PRIMARY KEY (`trnExpenseId`),
    CONSTRAINT `fk_expense_campaign` FOREIGN KEY (`campaignId`) REFERENCES `mstCampaigns`(`mstCampaignId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Table: trnDeliverables
-- ============================================================
CREATE TABLE IF NOT EXISTS `trnDeliverables` (
    `trnDeliverableId` INT NOT NULL AUTO_INCREMENT,
    `campaignId` INT NOT NULL,
    `influencerId` INT NULL,
    `deliverable` VARCHAR(255) NOT NULL,
    `completedOn` DATETIME NULL,
    `deadline` DATETIME NULL,
    `isCompleted` TINYINT(1) NOT NULL DEFAULT 0,
    `evidence` VARCHAR(255) NULL,
    `isActive` TINYINT(1) NOT NULL DEFAULT 1,
    PRIMARY KEY (`trnDeliverableId`),
    CONSTRAINT `fk_deliverable_campaign` FOREIGN KEY (`campaignId`) REFERENCES `mstCampaigns`(`mstCampaignId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Table: mstStakeholders
-- ============================================================
CREATE TABLE IF NOT EXISTS `mstStakeholders` (
    `mstStakeholderId` INT NOT NULL AUTO_INCREMENT,
    `campaignId` INT NULL,
    `name` VARCHAR(255) NOT NULL,
    `role` VARCHAR(100) NULL,
    `company` VARCHAR(100) NULL,
    `mobile` VARCHAR(20) NULL,
    `email` VARCHAR(255) NULL,
    `location` VARCHAR(100) NULL,
    `isActive` TINYINT(1) NOT NULL DEFAULT 1,
    `created_at` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`mstStakeholderId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================================
-- Seed Data: Campaigns
-- ============================================================
INSERT INTO `mstCampaigns` (`mstCampaignId`, `name`, `campaignType`, `campaignCycle`, `startDate`, `endDate`, `duration`, `totalSpend`, `conversionRate`, `downloadsBefore`, `downloadsAfter`, `objective`, `targetAudience`, `budget`, `creatorName`, `socialMediaPlatforms`, `status`, `isActive`) VALUES
(1, 'Fiestaa Campaign', 'Digital', 'Monthly', '2026-01-14 00:00:00', '2026-01-30 00:00:00', 16, 25000.00, 5.50, 150000, 200000, 'Launch the new W21 app with digital marketing campaigns.', 'General Users', 50000.00, 'Arjun Kumar', 'Instagram', 'Active', 1),
(2, 'Yugam Campaign', 'Digital', 'Monthly', '2026-01-15 00:00:00', '2026-01-30 00:00:00', 15, 30000.00, 6.20, 200000, 250000, 'W21 Passport Launch promotion', 'Travelers', 60000.00, 'Venky', 'YouTube', 'Active', 1),
(3, 'Peppa Foodie', 'Digital', 'Monthly', '2026-01-16 00:00:00', '2026-01-30 00:00:00', 14, 25000.00, 4.80, 100000, 130000, 'Food delivery app collaboration', 'Food lovers', 40000.00, 'Peppa', 'Instagram', 'Active', 1),
(4, 'Irfan\'s View Video', 'Digital', 'Yearly', '2026-01-22 00:00:00', '2026-01-30 00:00:00', 9, 45000.00, 8.50, 350000, 500000, 'Drive targeted app installs by partnering with a niche YouTube creator whose audience consists of aspiring content creators and filmmakers.', 'Lorem Ipsum', 80000.00, 'Mohammed Irfan', 'YouTube, Instagram', 'Active', 1),
(5, 'Go Green Marathon', 'Digital', 'Monthly', '2026-01-26 00:00:00', '2026-01-30 00:00:00', 4, 25000.00, 3.80, 50000, 65000, 'Marathon promotion', 'Fitness enthusiasts', 30000.00, 'Green Team', 'YouTube', 'Active', 1),
(6, 'Tinta Foodie', 'Digital', 'Monthly', '2026-01-28 00:00:00', '2026-02-05 00:00:00', 8, 15000.00, 2.50, 20000, 25000, 'Tinta foodie campaign', 'Local audience', 20000.00, 'Tinta', 'Instagram', 'Active', 1);

-- ============================================================
-- Seed Data: Influencers
-- ============================================================
INSERT INTO `mstInfluencers` (`mstInfluencerId`, `name`, `category`, `location`, `dateOfBirth`, `gender`, `socialMediaPlatforms`, `creatorId`, `managerName`, `managerNumber`, `instagramProfile`, `whatsAppContact`, `integrationRequirements`, `exclusivityClause`, `contentDurationClause`, `paymentTerms`, `followers`, `engagement`, `avgViews`, `estConversion`, `estCostMin`, `estCostMax`, `reliabilityScore`, `notes`, `isActive`) VALUES
(1, 'Irfan\'s View', 'Food, Travel', 'Chennai', '1996-10-12 00:00:00', 'Male', 'YouTube, Instagram', 'CHYT01', 'Sohail', '9842158724', 'https://instagram.com/irfansview', '+919842158724', 'Minimum 60-sec mid-roll placement. App name mentioned twice verbally. Link in top 3 lines of description. Pinned comment required.', 'No competitor app mention for 30 days', 'Video must remain live for minimum 6 months', '₹ 45,000 total, 50% advance, 50% post verification', '245K', 7.20, '180K', 3, 2500.00, 4000.00, 4, 'Reliable and high quality work.', 1),
(2, 'Bodyzeal', 'Lifestyle, Fitness', 'Chennai', '1995-05-15 00:00:00', 'Male', 'Instagram', 'CHIG02', 'Rahul', '9842158725', 'https://instagram.com/bodyzeal', '+919842158725', 'Product integration in fitness videos.', 'No competitor fitness app mention for 15 days', 'Post must remain live for 3 months', '100% post verification', '180K', 5.80, '90K', 2, 1500.00, 3000.00, 5, 'Great engagement with fitness audience.', 1);

-- ============================================================
-- Seed Data: Expenses
-- ============================================================
INSERT INTO `trnExpenses` (`campaignId`, `expenseName`, `category`, `paidTo`, `amount`, `status`, `expenseDate`, `proofPath`) VALUES
(4, 'Influencer Fee', 'Influencers', 'Influencers', 45000.00, 'Paid', '2026-02-12 00:00:00', 'Invoice.pdf');

-- ============================================================
-- Seed Data: Deliverables
-- ============================================================
INSERT INTO `trnDeliverables` (`campaignId`, `influencerId`, `deliverable`, `completedOn`, `deadline`, `isCompleted`, `evidence`) VALUES
(4, 1, 'YouTube Video', '2026-01-30 00:00:00', '2026-01-30 00:00:00', 1, 'Link'),
(4, 1, 'Community Post', '2026-01-30 00:00:00', '2026-01-30 00:00:00', 1, 'Attach'),
(4, 1, 'Instagram Reel', '2026-01-30 00:00:00', '2026-01-30 00:00:00', 1, 'Attach'),
(4, 1, 'Instagram Posts', '2026-01-30 00:00:00', '2026-01-30 00:00:00', 1, 'Attach'),
(4, 1, 'Instagram Story', '2026-01-30 00:00:00', '2026-01-30 00:00:00', 1, 'Screenshot.png'),
(4, 1, 'Instagram Story', NULL, '2026-01-30 00:00:00', 0, 'Attach'),
(4, 1, 'Instagram Story', NULL, '2026-01-30 00:00:00', 0, 'Attach'),
(4, 1, 'Instagram Story', NULL, '2026-01-30 00:00:00', 0, 'Attach');

-- ============================================================
-- Seed Data: Stakeholders
-- ============================================================
INSERT INTO `mstStakeholders` (`campaignId`, `name`, `role`, `company`, `mobile`, `email`, `location`) VALUES
(4, 'Arjun Kumar', 'Poster Designer', 'Freelance', '+91 98420 56897', 'arjundesigner@gmail.com', 'Chennai'),
(4, 'Venky', 'Travel Guide', 'SA Travels', '+91 98762 59862', 'venky@gmail.com', 'Chennai');

-- ============================================================
-- Schema Updates: mstCampaigns - new fields
-- ============================================================
ALTER TABLE `mstCampaigns`
    ADD COLUMN IF NOT EXISTS `influencerId` INT NULL AFTER `socialMediaPlatforms`,
    ADD COLUMN IF NOT EXISTS `basePay` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 AFTER `influencerId`,
    ADD COLUMN IF NOT EXISTS `incentiveAmount` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 AFTER `basePay`,
    ADD COLUMN IF NOT EXISTS `budgetThreshold` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 AFTER `incentiveAmount`,
    ADD COLUMN IF NOT EXISTS `allowance` DECIMAL(18, 2) NOT NULL DEFAULT 0.00 AFTER `budgetThreshold`,
    ADD COLUMN IF NOT EXISTS `termsAndConditions` TEXT NULL AFTER `allowance`,
    ADD COLUMN IF NOT EXISTS `influencerTag` VARCHAR(255) NULL AFTER `termsAndConditions`,
    ADD COLUMN IF NOT EXISTS `endReason` TEXT NULL AFTER `influencerTag`,
    ADD COLUMN IF NOT EXISTS `totalReach` INT NOT NULL DEFAULT 0 AFTER `endReason`;

-- ============================================================
-- Schema Updates: mstInfluencers - new fields
-- ============================================================
ALTER TABLE `mstInfluencers`
    ADD COLUMN IF NOT EXISTS `phoneNumber` VARCHAR(20) NULL AFTER `notes`,
    ADD COLUMN IF NOT EXISTS `shortDescription` TEXT NULL AFTER `phoneNumber`,
    ADD COLUMN IF NOT EXISTS `languagesFamiliar` VARCHAR(500) NULL AFTER `shortDescription`,
    ADD COLUMN IF NOT EXISTS `profilePicturePath` VARCHAR(500) NULL AFTER `languagesFamiliar`,
    ADD COLUMN IF NOT EXISTS `instagramUrl` VARCHAR(500) NULL AFTER `profilePicturePath`,
    ADD COLUMN IF NOT EXISTS `residentialAddress` TEXT NULL AFTER `instagramUrl`,
    ADD COLUMN IF NOT EXISTS `dateOfOnboarding` DATETIME NULL AFTER `residentialAddress`,
    ADD COLUMN IF NOT EXISTS `influencerInterests` VARCHAR(500) NULL AFTER `dateOfOnboarding`,
    ADD COLUMN IF NOT EXISTS `paymentDetails` TEXT NULL AFTER `influencerInterests`;
