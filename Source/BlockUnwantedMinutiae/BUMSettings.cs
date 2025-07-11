﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Verse;

namespace BlockUnwantedMinutiae;

public class BUMSettings : ModSettings
{
    public readonly bool[] GenericAlertValues = new bool[GenericAlertLabels.Count];
    public readonly bool[] GenericLetterValues = new bool[GenericLetterLabels.Count];

    public readonly bool[] GenericMessageValues = new bool[GenericMessageLabels.Count];
    private List<string> activeAlertPatches;
    private List<string> activeLetterPatches;
    private List<string> activeMessagePatches;

    private List<string> customFilters = [];
    public bool DrawAutoSelectCheckboxPatch = true;
    public bool IdleColonistsPatch = true;
    private List<string> seenText = [];

    public bool TaintedMessagePatch = true;

    public List<string> CustomFilters
    {
        get
        {
            customFilters ??= [];

            return customFilters;
        }
        private set => customFilters = value;
    }

    public List<string> SeenText
    {
        get
        {
            seenText ??= [];

            return seenText;
        }
        private set => seenText = value;
    }

    public static IReadOnlyList<string> GenericMessageLabels { get; } =
    [
        // Messages.xml
        // CORE
        "MessageSiegersAssaulting",
        "MessageRaidersBeginningAssault",
        "MessageRaidersDetectedEarlyAssault",
        "MessageRaidersKidnapping",
        "MessageRaidersStealing",
        "MessageCaravanDetectedRaidArrived",
        "MessageRaidersLeaving",
        "MessageRaidersGivenUpLeaving",
        "MessageRaidersSatisfiedLeaving",
        "MessageFightersFleeing",
        "MessageFriendlyFightersLeaving",
        "MessageVisitorsTakingWounded",
        "MessageVisitorsTrappedLeaving",
        "MessageVisitorsDangerousTemperature",
        "MessageWornApparelDeterioratedAway",
        "MessageDeterioratedAway",
        "MessageSeasonBegun",
        "MessageShipHasLeftCommsRange",
        "MessageNeedBeaconToTradeWithShip",
        "MessageNeedRoyalTitleToCallWithShip",
        "MessageConditionCauserDespawned",
        "MessageBillComplete",
        "MessageFullyHealed",
        "MessagePrisonerIsEscaping",
        "MessageOutOfNearbyShellsFor",
        "MessageOutOfNearbyFuelFor",
        "MessageAnimalsGoPsychoHunted",
        "MessageAnimalManhuntsOnTameFailed",
        "MessageInsectTameDesignated",
        "MessageInsectsHostileOnTaming",
        "LetterLabelMessageRecruitSuccess",
        "MessageRecruitSuccess",
        "MessageTameSuccess",
        "MessageTameAndNameSuccess",
        "MessageTameNoSuitablePens",
        "MessageRecruitJoinOfferAccepted",
        "MessageColonyCannotAfford",
        "MessageColonyNotEnoughSilver",
        "MessageCriticalAlert",
        "MessageMustDesignateHarvestable",
        "MessageMustDesignateHarvestableWood",
        "MessageMustDesignatePlants",
        "MessageMustDesignateHaulable",
        "MessageMustDesignateMineable",
        "MessageMustDesignateHuntable",
        "MessageMustDesignateTameable",
        "MessageMustDesignateClaimable",
        "MessageMustDesignatePaintableBuildings",
        "MessageMustDesignatePaintableFloors",
        "MessageMustDesignatePainted",
        "MessageMustDesignateDeconstructibleMechCluster",
        "MessageMustDesignateSmoothableSurface",
        "MessageNothingCanRemoveThickRoofs",
        "MessageAlreadyInStorage",
        "MessageMustDesignateStrippable",
        "MessageMustDesignateSlaughterable",
        "MessageMustDesignateOpenable",
        "MessageMustDesignateForbiddable",
        "MessageMustDesignateUnforbiddable",
        "MessageCannotClaimWhenThreatsAreNear",
        "MessageCannotAdoptWhileThreatsAreNear",
        "MessageRefusedArrest",
        "MessageNoMedicalBeds",
        "MessageNoAnimalBeds",
        "MessageWarningNoMedicineForRestriction",
        "PawnDiedBecauseOf",
        "PawnDied",
        "MessageNoLongerDowned",
        "MessageInvoluntarySleep",
        "MessageMedicalOperationWillAngerFaction",
        "MessageAnimalIsPregnant",
        "MessageMiscarriedStarvation",
        "MessageMiscarriedPoorHealth",
        "MessageGaveBirth",
        "MessagePsylinkNoSensitivity",
        "MessageLostImplantLevelFromHediff",
        "MessageReceivedBrainDamageFromHediff",
        "MessageWentOverPsychicEntropyLimit",
        "MessageNoHandlerSkilledEnough",
        "MessageEatenByPredator",
        "MessageAttackedByPredator",
        "MessageRoamerLeaving",
        "MessageHiveReproduced",
        "MessageTraderCaravanLeaving",
        "MessageCantSelectDeadPawn",
        "MessageCantSelectOffMapPawn",
        "MessageSocialFight",
        "MessageNewMarriageCeremony",
        "MessageMarriageCeremonyStarts",
        "MessageMarriageCeremonyCalledOff",
        "MessageNewlyMarried",
        "MessageMarriageCeremonyAfterPartyFinished",
        "MessageNewBondRelation",
        "MessageNewBondRelationNewName",
        "MessageBondedAnimalMentalBreak",
        "MessageNamedBondedAnimalMentalBreak",
        "MessageBondedAnimalsMentalBreak",
        "MessageSuccessfullyRemovedHediff",
        "MessageShipChunkDrop",
        "MessageCannotSellItemsReason",
        "MessageConstructionFailed",
        "MessageScreenshotSavedAs",
        "MessageNoLongerSocialFighting",
        "MessageNoLongerBingingOnDrug",
        "MessageNoLongerOnTargetedInsultingSpree",
        "MessageRottedAwayInStorage",
        "MessageFoodPoisoning",
        "MessagePawnLostWhileFormingCaravan",
        "MessagePawnLostWhileFormingCaravan_AllLost",
        "MessageCaravanFormationPaused",
        "MessageCaravanFormationUnpaused",
        "MessageCaravanMemberHasExtremeMentalBreak",
        "MessageMaxPlanetCoveragePerformanceWarning",
        "MessagePlantDiedOfCold",
        "MessagePlantDiedOfRot_LeftUnharvested",
        "MessagePlantDiedOfRot_ExposedToLight",
        "MessagePlantDiedOfRot",
        "MessagePlantDiedOfPoison",
        "MessagePlantDiedOfBlight",
        "MessagePawnBeingBurned",
        "MessageAccidentalOverdose",
        "MessageCaravanArrivedAtDestination",
        "MessagePawnLeftMapAndCreatedCaravan",
        "MessagePawnLeftMapAndCreatedCaravan_AnimalsWantToJoin",
        "MessageSettledInExistingMap",
        "MessagePermanentWoundHealed",
        "MessageRescueeDidntJoin",
        "MessageTransporterUnreachable",
        "MessageTransportersNotAdjacent",
        "MessageTransportersLoadCanceled_TransporterDestroyed",
        "MessageTransporterSingleLoadCanceled_TransporterDestroyed",
        "MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned",
        "MessageTransportersLoadingProcessStarted",
        "MessageTransporterSingleLoadingProcessStarted",
        "MessageTransportPodsArrived",
        "MessageTransportPodsArrivedAndLost",
        "MessageTransportPodsArrivedAndAddedToCaravan",
        "MessageFinishedLoadingTransporters",
        "MessageFinishedLoadingTransporterSingle",
        "MessageCantLoadMoreIntoTransporters",
        "MessageTransportPodsDestinationIsInvalid",
        "MessageTransportPodsDestinationIsTooFar",
        "MessageReformedCaravan",
        "MessageCantBanishLastColonist",
        "MessageCantEquipCustom",
        "MessageCantEquipIncapableOfViolence",
        "MessageCantEquipIncapableOfShooting",
        "MessageCantEquipIncapableOfManipulation",
        "MessageCantWearApparelMissingBodyParts",
        "MessageCantUnequipLockedApparel",
        "MessageWouldReplaceLockedApparel",
        "MessageSlaughteringBondedAnimal",
        "MessageReleaseBondedAnimal",
        "MessageCaravanRanOutOfFood",
        "MessageCaravanDeathCorpseAddedToInventory",
        "MessageSelectOwnBaseToFormCaravan",
        "MessageScreenResTooSmallForUIScale",
        "MessageYouHaveToReformCaravanNow",
        "MessageDefendersAttacking",
        "MessageMechanoidsAssembled",
        "MessageMechanoidsLeftToAssemble",
        "MessageMechanoidsReinforcementsDrop",
        "MessageSelfTendUnsatisfied",
        "MessageCannotSelfTendEver",
        "MessageCantAddWaypointBecauseImpassable",
        "MessageCantAddWaypointBecauseUnreachable",
        "MessageCantAddWaypointBecauseLimit",
        "MessageCantRemoveWaypointBecauseFirst",
        "MessageCantDoExecutionBecauseNoWardenCapableOfViolence",
        "MessageCanReformCaravanNowNoMoreEnemies",
        "MessageCanReformCaravanNowNoMoreEnemiesButUnexploredAreas",
        "MessageCantBanishDownedPawn",
        "MessageSleepingPawnsWokenUp",
        "MessageMechClusterDefeated",
        "MessageCompSpawnerSpawnedItem",
        "MessageHediffCuredByItem",
        "MessageBodyPartCuredByItem",
        "MessageResearchProjectFinishedByItem",
        "MessagePawnResurrected",
        "MessageFailedToRescueRelative",
        "MessageRescuedRelative",
        "MessageTornadoLeftMap",
        "MessageTornadoDissipated",
        "MessagePeaceTalksNoDiplomat",
        "MessageWarningCavePlantsExposedToLight",
        "MessageTargetedTantrumChangedTarget",
        "MessageTargetedInsultingSpreeChangedTarget",
        "MessageMurderousRageChangedTarget",
        "MessagePlantIncompatibleWithRoof",
        "MessageRoofIncompatibleWithPlant",
        "MessagePlayerTriedToLeaveMapViaExitGrid_CanReform",
        "MessagePlayerTriedToLeaveMapViaExitGrid_CantReform",
        "MessageGoodwillChanged",
        "MessageGoodwillChangedWithReason",
        "MessageGiftGivenButNotAppreciated",
        "MessageCantGiveGiftBecauseCantCarryEncumbered",
        "MessageCantGiveGiftBecauseCantCarry",
        "MessageFormedCaravan",
        "MessageFormedCaravan_Orders",
        "MessageBillValidationStoreZoneDeleted",
        "MessageBillValidationStoreZoneUnavailable",
        "MessageBillValidationPawnUnavailable",
        "MessageBillValidationIncludeZoneDeleted",
        "MessageBillValidationIncludeZoneUnavailable",
        "MessageBillValidationStoreZoneInsufficient",
        "MessageAnimalReturnedWild",
        "MessageAnimalReturnedWildReleased",
        "MessageAnimalLostSkill",
        "MessageTranslationReportSaved",
        "MessageCaravanArrivalActionNoLongerValid",
        "MessageEnterCooldownBlocksEntering",
        "MessagePermitCooldownFinished",
        "MessagePredatorHuntingPlayerAnimal",
        "MessagePrisonerResistanceBroken",
        "MessagePrisonerResistanceBroken_RecruitAttempsWillBegin",
        "MessageModWithPackageIdAlreadyEnabled",
        "MessageDisableModsBeforeCleaningTranslationFiles",
        "MessageUnpackBeforeCleaningTranslationFiles",
        "MessageTranslationFilesCleanupDone",
        "MessageCantCleanupTranslationFilesBeucaseOfXmlError",
        "MessageCapturingWillAngerFaction",
        "MessageStrippingWillAngerFaction",
        "MessageCantShootInMelee",
        "MessageNoColonistCanAcceptQuest",
        "MessageCannotAcceptQuest",
        "MessageQuestAccepted",
        "MessagePawnLeaving",
        "MessagePawnsLeaving",
        "MessageMonumentDestroyedBecauseOfDisallowedBuilding",
        "MessageNoResearchBenchForTechprint",
        "NoValidDestinationFound",
        "MessageMinifiedTreeDied",
        "MessageResearchMenuWithoutBench",
        "MessageActivatorProximityTriggered",
        "MessageCannotSelectInvisibleStat",
        "MessagePrisonerCannotEquipWeapon",
        "MessageIdeoOpposedWorkTypeSelected",
        "MessageBedLostAssignment",
        "MessageBedDestroyed",
        "Spectate",
        "AssignToRole",
        "RitualBegun",
        "MaxPawnsPerRole",
        "RoleIsLocked",
        "MessageRitualNeedsAtLeastOnePerson",
        "MessageRitualNeedsAtLeastOneSpectator",
        "MessageRitualNeedsAtLeastOneRolePawn",
        "MessageRitualNeedsAtLeastNumRolePawn",
        "MessageRitualPawnDowned",
        "MessageRitualPawnPrisonerNotSecured",
        "MessageRitualPawnSlaveNotSecured",
        "MessageRitualPawnReleased",
        "MessageRitualPawnInjured",
        "MessageRitualRoleRequired",
        "MessageRitualRoleMustBePrisoner",
        "MessageRitualRoleMustBeAnimal",
        "MessageRitualRoleMustBeHumanlike",
        "MessageRitualRoleCannotBeABaby",
        "MessageRitualNoRolesAvailable",
        "MessageRitualNotOfPlayerIdeo",
        "MessageRitualRoleMustHaveEyes",
        "MessageRitualRoleMustRequireScarification",
        "MessageRitualRoleMustRequireBlinding",
        "MessageRitualRoleMustBePrisonerOrSlave",
        "MessageRitualRoleMustBeCapableOfFighting",
        "MessageRitualRoleCannotReplaceRequiredPawn",
        "MessageRitualRolePawnRecentlyTornConnection",
        "MessageRitualRolePawnHasSameIdeo",
        "MessageRitualRoleMustBeCapableOfWardening",
        "MessageRitualRoleMustBeCapableOfGeneric",
        "MessageRitualRoleMustNotBePrisonerToSpectate",
        "MessageRitualRoleMustHaveIdeoToSpectate",
        "MessageRitualRoleMustHaveIdeoToDoRole",
        "MessageRitualRoleMustBeColonist",
        "MessageRitualWontAttendExtremeTemperature",
        "MessageRitualRoleMustBeFreeColonist",
        "MessageRitualRoleMustHaveLargerBodySize",
        "MessageRitualPawnMentalState",
        "MessageRitualPawnIsAlreadyBelievingIdeo",
        "MessageRitualCannotAssignAnyRoleFromSpectating",
        "MessageRitualRoleMustNotBeImprisonedWildMan",
        "MessageRitualRoleMustNotBeImprisoned",
        "MessageRitualRoleCannotBeChild",
        "MessageRitualRoleCannotBeBaby",
        "MessagePlayerMustSelectTile",
        "CannotPlantThing",
        "MessageWarningNotEnoughFertility",
        "BlockedBy",
        "AdjacentSowBlocker",
        "TooCloseToOtherPlant",
        "TooCloseToOtherSeedPlantCell",
        "MessageWarningCutImportantPlant",
        "MessageIncapableOfManipulation",
        "MessageActivationCanceled",
        "NothingAvailableInCategory",
        "SettingsLinkedFor",
        "SettingsUnlinkedFor",
        "StorageSettingsCopiedToClipboard",
        "StorageSettingsPastedFromClipboard",
        // ROYALTY
        "ShuttleBlocked",
        "MessageShuttleArrived",
        "MessageShuttleArrivedContentsLost",
        "CannotCallShuttle",
        "ShuttleCannotLand_Fogged",
        "ShuttleCannotLand_Unwalkable",
        "MessageBestowerWaiting",
        "MessageBestowerUnreachable",
        "MessageBestowingTargetIsBusy",
        "MessageBestowingSpotUnreachable",
        "MessageBestowingDanger",
        "MessageBestowingDangerTemperature",
        "MessageBestowingInterrupted",
        "MessageBestowingCeremonyStarted",
        "MessageMissionGetBackToShuttle",
        "MessagePermitTransportDrop",
        "MessagePermitTransportDropCaravan",
        "MessageNoFactionForVerbMechCluster",
        // IDEOLOGY
        "MessageMustChooseIdeo",
        "MessageIdeoNameCantBeEmpty",
        "MessageIdeoIncompatiblePrecepts",
        "MessageIdeoWarnRoleApparelOverlapsDesiredApparel",
        "MessageRitualMissingTarget",
        "MessageBuildingMissingRitual",
        "MessagePawnUnwillingToDoDueToIdeo",
        "MessageFailedConvertIdeoAttempt",
        "MessageFailedConvertIdeoAttemptSocialFight",
        "MessageNotEnoughMemes",
        "MessageTooManyMemes",
        "MessageNotEnoughStructureMemes",
        "MessageIncompatibleMemes",
        "MessageNoRequiredRolePawnToBeginRitual",
        "MessageNotEnoughSpectators",
        "MessageRoleChangeChooseDifferentRole",
        "MessageNeedAssignedRoleToBeginRitual",
        "MessageNeedAtLeastOneParticipantOfIdeo",
        "MessageRoleAssigned",
        "MessageRoleUnassignedPrisoner",
        "MessageWarningPlayerDesignatedTreeChopped",
        "MessageWarningPlayerDesignatedMining",
        "MessageConnectedPawnDied",
        "MessageSlaveEmancipated",
        "MessagePrisonerEnslaved",
        "MessagePrisonerWillBroken",
        "MessagePrisonerWillBroken_RecruitAttempsWillBegin",
        "MessageNoWardenCapableOfEnslavement",
        "MessageNoWardenOfIdeo",
        "MessageCampDetected",
        "MessageCharityEventRefused",
        "MessageCharityEventFulfilled",
        "MessageBeggarsLeavingWithNoItems",
        "MessageBeggarsLeavingWithItems",
        "MessageWandererLeftToDie",
        "MessageWandererRecruited",
        "MessageWandererLeftHealthy",
        "MessageCharityQuestEndedFailed",
        "MessageCharityQuestEndedSuccess",
        "CannotRemove",
        "CannotRemoveMemeRequired",
        "CannotRemoveMemeRequiredPlayer",
        "RequiredByFaction",
        "OverlappingRoleApparel",
        "SelfDestructCountdown",
        "PreceptNameTooLong",
        "MessageAncientTerminalDiscovered",
        "MessageAncientComplexBackToShuttle",
        "MessageAncientSignalActivated",
        "MessageAncientSignalHostileDetected",
        "MessageAncientAltarThreatsWokenUp",
        "MessageAncientAltarThreatsAlerted",
        "MessageAnimalIsVeneratedForAllColonists",
        "MessageNewColonyMax",
        "MessageNewColoyRequiresOneColonist",
        "MessageFuelNodeTriggered",
        "MessageSleepingThreatDelayActivated",
        "MessageInfestationDelayActivated",
        "MessageFuelNodeDelayActivated",
        "MessageDevelopmentPointsEarned",
        "MessageFluidIdeoOneChangeAllowed",
        // BIOTECH
        "MessageColonistReaching2ndTrimesterPregnancy",
        "MessageColonistReaching3rdTrimesterPregnancy",
        "MessageColonistInFinalStagesOfLabor",
        "MessageMechChargerDestroyedMechGoesBerserk",
        "MessageCantUseOnResistingPerson",
        "MessageCannotUseOnSameXenotype",
        "MessageCanOnlyTargetColonistsPrisonersAndSlaves",
        "MessageCannotImplantInTempFactionMembers",
        "MessageXenogermCompleted",
        "MessageXenogermCancelledMissingPack",
        "MessageAbsorbingXenogermWillAngerFaction",
        "MessageCannotUseOnOtherFactions",
        "MessageCaravanAddingEscortingMech",
        "MessageCaravanRemovingEscortingMech",
        "MessageMechanitorCasketOpened",
        "MessageNoBloodfeedersPrisonerInteractionReset",
        "MessagePregnancyTerminated",
        "MessageWarningPollutedCell",
        "MessageWarningNotPollutedCell",
        "MessagePlantDiedOfPollution",
        "MessagePlantDiedOfNoPollution",
        "MessageWorldTilePollutionChanged",
        "MessageCocoonDisturbed",
        "MessageCocoonDisturbedPlural",
        "MessageTargetMustBeDownedToForceReimplant",
        "MessageWorkTypeDisabledAge",
        "MessageDeathrestingPawnCanWakeSafely",
        "MessageMechanitorLostControlOfMech",
        "MessageMechanitorDisconnectedFromMech",
        "MetabolismTooLowToCreateXenogerm",
        "ComplexityTooHighToCreateXenogerm",
        "MessageNoSelectedGenepacks",
        "MessageNoSelectedGenes",
        "CanOnlyStoreNumGenepacks",
        "XenotypeNameCannotBeEmpty",
        "MessageGeneMissingPrerequisite",
        "MessageDeathrestCapacityChanged",
        "SanguophagesArrivingSoon",
        "SanguophagesBegunMeeting",
        "SanguophagesLeavingTemperature",
        "MessagePawnWokenFromSunlight",
        "MessageBedExposedToSunlight",
        "MessagePawnKilledRipscanner",
        "MessageCannotResurrectDessicatedCorpse",
        "MessageAboutToExplode",
        "MessageChildcareDisabled",
        "MessageChildcareNotAssigned",
        "MessageTooManyCustomXenotypes",
        "MessageConflictingGenesPresent",
        "MessageDeathrestBuildingBound",
        "MessageDevelopmentalStageSelectionDisabledByScenario",
        "MessagePawnHadNotEnoughBloodToProduceHemogenPack",
        "MessageCannotStartHemogenExtraction",
        "MessageCannotPostponeGrowthMoment",
        "MessageDraftedPawnCarryingBaby",
        "MessageTakingBabyToSafeTemperature",
        // ANOMALY
        "MessageActivityRisingDamage",
        "EntityDiedOnHoldingPlatform",
        "MessageEntityDiscovered",
        "MessageHateChantersAbsorbed",
        "MessageHateChantIncreased",
        "SelfResurrectText",
        "MessageUsingSelfResurrection",
        "MessagePitGateCollapsed",
        "MessagePitBurrowCollapsed",
        "FleshmassContainedMessage",
        "MessagePsychicRitualAssault",
        "MessageAIPsychicRitualBegan",
        "MessageBiomutationLanceInvalidTargetRace",
        "MessageBiomutationLanceTargetTooBig",
        "MessageNoRoomWithMinimumContainmentStrength",
        "MessageTargetBelowMinimumContainmentStrength",
        "MessageHolderReserved",
        "MessageEntityExecuted",
        "MessageOccupiedHoldingPlatformReinstalled",
        "MessageOccupiedHoldingPlatformUninstalled",
        "MessageOccupiedHoldingPlatformDeconstructed",
        "MessageHostileDuplicate",
        "MessageProximityDetectorTriggered",
        "MessagePawnAttackedInDarkness",
        "MessageRevenantForcedVisibility",
        "MessageRevenantHeard",
        "MessageCannotUseOnNonBleeder",
        "MessageFingerspikeDisturbed",
        "MessageFingerspikeDisturbedPlural",
        "DeathPallResurrectedMessage",
        // CORE
        // DamageDefs
        "MessageDeathByBurning",
        "MessageDeathByFrostbite",
        "MessageDeathByTornado",
        "MessageDeathBySurgery",
        "MessageDeathByExecution",
        "MessageDeathByCutting",
        "MessageDeathByCrushing",
        "MessageDeathByBeating",
        "MessageDeathByStabbing",
        "MessageDeathByTearing",
        "MessageDeathByBiting",
        "MessageDeathByExplosion",
        "MessageDeathByShot",
        "MessageDeathByArrow",
        "MessageDeathByStun",
        "MessageDeathByEMP",
        // GameConditions_Misc.xml
        "MessageSolarFlare",
        "MessageEclipse",
        "MessagePsychicDrone",
        "MessagePsychicSoothe",
        "MessageToxicFallout",
        "MessageVolcanicWinter",
        "MessageHeatWave",
        "MessageColdSnap",
        "MessageFlashstorm",
        "MessageAurora",
        // Gatherings.xml
        "MessagePartyCalledOff",
        "MessagePartyFinished",
        // HediffDefs
        "MessagePregnant",
        "MessageOverdose",
        "MessageExcisedCarcinoma",
        "MessageCuredScaria",
        "MessageSterilized",
        // Inspirations.xml
        "MessageEndedInspireWorkFrenzy",
        "MessageEndedInspireGoFrenzy",
        "MessageEndedInspireShootFrenzy",
        "MessageEndedInspireTrade",
        "MessageEndedInspireRecruitment",
        "MessageEndedInspireTaming",
        "MessageEndedInspireSurgery",
        "MessageEndedInspireCreativity",
        // MentalStateDefs
        "MessageRecoveryBerserk",
        "MessageRecoveryArson",
        "MessageRecoveryEscape",
        "MessageRecoverySlaughter",
        "MessageRecoveryMurderous",
        "MessageRecoveryReturn",
        "MessageRecoveryPsyWander",
        "MessageRecoveryTantrum",
        "MessageRecoverySadistic",
        "MessageRecoveryCorpse",
        "MessageRecoveryGlutton",
        "MessageRecoverySadWander",
        "MessageRecoveryHiding",
        "MessageRecoveryInsulting",
        "MessageRecoveryConfused",
        "MessageRecoveryFleeing",
        "MessageRecoveryManhunting",
        // ThingDefs_Buildings
        "MessagePlantResting",
        "MessageNeedsNewBarrel",
        "MessageNoSlugs",
        "MessageNeedsNewReinforcedBarrel",
        "MessageNoFoam",
        "MessageNoRockets",
        // Misc_Gameplay.xml
        "BladelinkAlreadyBondedMessage",
        "WillGetWorkSpeedPenalty",
        // Dialogs_Various.xml
        "MessageMustChooseRouteFirst",
        "MessageNoValidExitTile",
        // GameplayCommands.xml
        "MessageTargetBeyondMaximumRange",
        "MessageTargetBelowMinimumRange",
        "MessageTurretWontFireBecauseHoldFire",
        // Incidents.xml
        "MessageAnimalInsanityPulse",
        // Misc_Gameplay.xml
        "MessageColonistMarkedForExecution",
        // FloatMenu.xml	
        "NoPrisonerBed",
        "NoNonPrisonerBed",
        "NoAnimalBed",
        // ROYALTY
        // Gatherings.xml
        "MessageConcertCalledOff",
        "MessageConcertFinished",
        // Hediffs_Local_Misc.xml
        "MessageCuredBloodRot",
        "MessageCuredAbasia",
        // Ritual_Behaviors.xml
        "MessageAdultsAnima",
        // Buildings_ConditionCausers.xml
        "MessageWeatherFinished",
        "MessageSupressorFinished",
        "MessageEMIFinished",
        // Plants_Wild.xml
        "MessageAnimaDied",
        // Misc_Gameplay.xml
        "MessageShuttleDestinationIsTooFar",
        // Script_BuildMonument_Worker.xml
        "MessageMonumentViolated",
        // IDEOLOGY
        // Abilities.xml
        "MessageConvertSuccess",
        "MessageConvertFailure",
        "MessageReassureSuccess",
        "MessageCounselSuccess",
        "MessageCounselSuccessNoNeg",
        "MessageCounselFailure",
        "MessageAnimalCalmSuccess",
        // Ritual_Behaviors.xml
        "MessageAdultsGauranlen",
        "RitualStageActionMessage",
        // Misc_Gameplay.xml
        "BiosculpterEnteringMessage",
        "BiosculpterLoadingStartedMessage",
        "BiosculpterCarryStartedMessage",
        "BiosculpterNoPowerEjectedMessage",
        "BiosculpterHealCompletedMessage",
        "BiosculpterHealCompletedWithCureMessage",
        "BiosculpterAgeReversalCompletedMessage",
        "BiosculpterPleasureCompletedMessage",
        // Script_ReliquaryPilgrims.xml
        "PilgrimsLeavingMessage",
        "PilgrimsLeftMessage",
        "TerminalHackedMessage",
        "TerminalHackedMultiMessage",
        "AllTerminalsHackedMessage",
        // BIOTECH
        // DamageDefs
        "MessageDeathByBeam",
        "MessageDeathByHeat",
        "MessageDeathByShock",
        // GameConditions_Misc.xml
        "MessageAcidicSmogStart",
        "MessageAcidicSmogEnd",
        // Hediffs_Global_Misc.xml
        "MessageLaborProgressing",
        "MessageInfantIllness",
        "MessageNotBreastfeeding",
        "MessageNotLactating",
        // Hediffs_Various.xml
        "MessageGeneRegrowing",
        // MentalStates_Special.xml
        "MessageRecoveryFire",
        "MessageRecoveryRage",
        // Recipes_Surgery_Misc.xml
        "MessagePerformedLitigation",
        "MessagePerformedVasectomy",
        "MessagePerformedReversal",
        "MessageImplantedIUD",
        "MessageRemovedIUD",
        // ITabs.xml
        "TryRomanceNoOptsMessage",
        "TryRomanceFailedMessage",
        // Misc_Gameplay.xml
        "ImplantFailedMessage",
        // ANOMALY
        // Misc_Gameplay.xml
        "MessageRitualCannotBeMutant",
        "MessageSightstealerRevealed",
        "MessageMetalHorrorDormant",
        "MessageTransmutedItem",
        "MessageTransmutedStuff",
        "MessageTransmutedStuffPlural",
        "MessageHeartAttack",
        "MessageShardDropped",
        "MessageCorpseEscaped",
        "MessageUnnaturalCorpseResurrect",
        "MessagePawnFainted",
        "MessageAwokenReappeared",
        "MessageAwokenVanished",
        "MessageAwokenAttacking",
        "MessageAwokenKilledVictim",
        "MessageAwokenDisappeared",
        "MessageGoldenCubeSeverityIncreased",
        "MessageGoldenCubeWithdrawal",
        "MessageGoldenCubeWithdrawalIncreased",
        "MessageGoldenCubeInterest",
        "MessageGoldenCubeSculptureDestroyed",
        "MessageSlaughterNoFlesh",
        "MessageSlaughterTooBig",
        "MessagePawnVanished",
        "MessagePawnReappeared",
        "MessageChimeraModeChangeSingular",
        "MessageChimeraModeChangePlural",
        "MessageChimeraWithdrawing"
    ];

    public static IReadOnlyList<string> GenericAlertLabels { get; } =
    [
        // CUSTOM
        "BreakRiskMinor",
        "BreakRiskMajor",
        "BreakRiskExtreme",
        "AlertTatteredApparel",
        "AlertUnhappyNudity",
        // CORE
        "ActivatorCountdown",
        "AlertAnimalFilth",
        "AlertAnimalPenNeeded",
        "AlertAnimalPenNotEnclosed",
        "AlertAnimalIsRoaming",
        "PatientsAwaitingMedicalOperation",
        "BilliardsTableOnWall",
        "Boredom",
        "BrawlerHasRangedWeapon",
        "BuildingCantBeUsedRoofed",
        "CaravanIdle",
        "AlertCasketOpening",
        "ColonistLeftUnburied",
        "ColonistNeedsRescuing",
        "ColonistNeedsTend",
        "ColonistsIdle",
        "DormancyWakeUpDelay",
        "Exhaustion",
        "FireInHomeArea",
        "FuelNodeIgnition",
        "Heatstroke",
        "HitchedAnimalHungryNoFood",
        "HunterHasShieldAndRangedWeapon",
        "HunterLacksRangedWeapon",
        "Hypothermia",
        "HypothermicAnimals",
        "ImmobileCaravan",
        "InfestationDelay",
        "JoyBuildingNoChairs",
        "LifeThreateningHediff",
        "LowFood",
        "LowMedicine",
        "MajorOrExtremeBreakRisk",
        "MinifiedTreeAboutToDie",
        "MinorBreakRisk",
        "NeedBatteries",
        "NeedColonistBeds",
        "NeedDefenses",
        "NeedDoctor",
        "NeedJoySources",
        "NeedMealSource",
        "NeedMiner",
        "NeedResearchBench",
        "NeedResearchProject",
        "NeedWarden",
        "NeedWarmClothes",
        "PasteDispenserNeedsHopper",
        "PennedAnimalHungry",
        "PredatorInPen",
        "QuestExpiresSoon",
        "ShieldUserHasRangedWeapon",
        "StarvationAnimals",
        "StarvationColonists",
        "Thought",
        "Alert",
        "Delay",
        "MoodBelow",
        "ShuttleDelay",
        "ShuttleLeaveDelay",
        // ROYALTY
        "AnimaLinkingReady",
        "BestowerWaiting",
        "DisallowedBuildingInsideMonument",
        "MonumentMarkerMissingBlueprints",
        "NeedMeditationSpot",
        "RoyalNoAcceptableFood",
        "RoyalNoThroneAssigned",
        "ShuttleLandingBeaconUnusable",
        "ThroneroomInvalidConfiguration",
        "TimedMakeFactionHostile",
        "TimedRaidsArriving",
        "TitleRequiresBedroom",
        "UndignifiedBedroom",
        "UndignifiedThroneroom",
        "UnusableMeditationFocus",
        "PermitAvailable",
        // IDEOLOGY
        "AgeReversalDemandNear",
        "ConnectedPawnNotAssignedToPlantCutting",
        "DateRitualComing",
        "GauranlenTreeWithoutProductionMode",
        "IdeoBuildingDisrespected",
        "IdeoBuildingMissing",
        "NeedSlaveBeds",
        "RolesEmpty",
        "SlaveRebellionLikely",
        "SlavesUnattended",
        "SlavesUnsuppressed",
        // BIOTECH
        "Biostarvation",
        "GenebankUnpowered",
        "LowBabyFood",
        "LowDeathrest",
        "LowHemogen",
        "MechChargerFull",
        "MechDamaged",
        "MechMissingBodyPart",
        "NeedBabyCribs",
        "NeedMechChargers",
        "NeedSlaveCribs",
        "NoBabyFeeders",
        "NoBabyFoodCaravan",
        "PollutedTerrain",
        "PsychicBondedSeparated",
        "ReimplantationAvailable",
        "SubjectHasNowOverseer",
        "ToxicBuildup",
        "ToxifierGeneratorStopped",
        "WarqueenHasLowResources",
        // ANOMALY
        "Alert_UndercaveUnstable",
        "Alert_CultistPsychicRitual",
        "Alert_InsufficientContainment",
        "AlertMeatHunger",
        "AlertHoldingPlatform",
        "AlertCubeWithdrawal",
        "AlertDigestion",
        "AlertInhibitorBlocked",
        "CreepJoinerTimeout",
        "NeedAnomalyProject",
        "ActivityMultipleDangerous",
        "EntityNeedsTreatment",
        "AlertGhoulHypothermia",
        "AlertAnalyzable"
    ];

    public static IReadOnlyList<string> GenericLetterLabels { get; } =
    [
        // Letters.xml
        // CORE
        "LetterLabelFirstSummerWarning",
        "LetterLabelAreaRevealed",
        "LetterLabelRoofCollapsed",
        "LetterLabelBirthday",
        "LetterLabelNewDisease",
        "LetterLabelAncientShrineWarning",
        "LetterLabelAnimalManhunterRevenge",
        "LetterFriendlyTrapSprungLabel",
        "LetterLeadersDeathLabel",
        "LetterLeaderChangedLabel",
        "LetterNewLeader",
        "LetterLabelNoticedRelatedPawns",
        "LetterLabelAffair",
        "LetterLabelNewLovers",
        "LetterLabelBreakup",
        "LetterLabelAcceptedProposal",
        "LetterLabelRejectedProposal",
        "LetterLabelRelationsChange_Hostile",
        "LetterLabelRelationsChange_Ally",
        "LetterLabelRelationsChange_NeutralFromHostile",
        "LetterLabelRelationsChange_NeutralFromAlly",
        "LetterLabelShortCircuit",
        "LetterLabelSuffixBondedAnimalDied",
        "LetterLabelPrisonBreak",
        "LetterLabelNewlyAddicted",
        "LetterLabelDrugBinge",
        "LetterLabelAllCaravanColonistsDied",
        "LetterLabelCaravanEnteredMap",
        "LetterLabelCaravanEnteredEnemyBase",
        "LetterLabelTransportPodsLandedInEnemyBase",
        "LetterLabelFactionBaseDefeated",
        "LetterLabelFoundPreciousLump",
        "LetterLabelDeepScannerFoundLump",
        "LetterLabelAmbushInExistingMap",
        "LetterLabelPeaceTalks_Disaster",
        "LetterLabelPeaceTalks_Backfire",
        "LetterLabelPeaceTalks_TalksFlounder",
        "LetterLabelPeaceTalks_Success",
        "LetterLabelPeaceTalks_Triumph",
        "LetterLabelAICoreOffer",
        "LetterCraftedLegendaryLabel",
        "LetterCraftedMasterworkLabel",
        "LetterLabelPawnsLostBecauseMapClosed_Caravan",
        "LetterLabelPawnsLostBecauseMapClosed_Home",
        "LetterLabelHibernateComplete",
        "LetterLabelVisitorsGaveGift",
        "LetterLabelFactionBaseProximity",
        "LetterLabelCaravansBattlefieldVictory",
        "LetterLabelRescueQuestFinished",
        "LetterLabelPredatorHuntingColonist",
        "LetterDisease_Blocked",
        "LetterLabelTraitDisease",
        "LetterLabelQuestDropPodsArrived",
        "LetterLabelQuestItemsAddedToCaravanInventory",
        "LetterLabelRefugeeJoins",
        "LetterLabelRescueeJoins",
        "LetterLabelPawnsArriveAndJoin",
        "LetterLabelPawnsArrive",
        "LetterLabelPawnLeaving",
        "LetterLabelPawnsLeaving",
        "LetterLabelPawnsKidnapped",
        "LetterTechprintAppliedLabel",
        "LetterQuestCompletedLabel",
        "LetterQuestFailedLabel",
        "LetterQuestConcludedLabel",
        "LetterBladelinkWeaponBondedLabel",
        "LetterLabelMechClusterArrived",
        "LetterLabelSiteCountdownStarted",
        "LetterLabelUnrecruitablePawnCaptured",
        "LetterJoinOfferLabel",
        // ROYALTY
        "LetterLabelLostRoyalTitle",
        "LetterLabelGainedRoyalTitle",
        "LetterLabelRewardsForNewTitle",
        "LetterTitleHeirLostLabel",
        "LetterTitleInheritance",
        "LetterLawViolationDetectedLabel",
        "LetterLabelSpeechCancelled",
        "WildDecree",
        "LetterLabelRandomDecree",
        "LetterLabelPsylinkLevelGained",
        "LetterLabelBestowingCeremonyExpired",
        "LetterLabelBestowingCeremonyTitleUpdated",
        "LetterLabelShuttleCrashed",
        "LetterLabelWandererJoinsAbasia",
        // IDEOLOGY
        "LetterLabelConvertIdeoAttempt_Success",
        "LetterLabelGrandSlaveRebellion",
        "LetterLabelLocalSlaveRebellion",
        "LetterLabelSingleSlaveRebellion",
        "LetterLabelGrandSlaveEscape",
        "LetterLabelLocalSlaveEscape",
        "LetterLabelSingleSlaveEscape",
        "LetterLabelRoleActive",
        "LetterLabelRoleInactive",
        "LetterLabelRoleLost",
        "LetterObligationRoleInactive",
        "LetterTitleObligationsActivated",
        "LetterTitleObligationsDeactivated",
        "LetterBondRemoved",
        "LetterLabelEnslavementSuccess",
        "LetterLabelNewPrimaryIdeo",
        "PawnsHaveCharitableBeliefs",
        "LetterIdeoChangedDivorce",
        "LetterLabelSurpriseReinforcements",
        "LetterWantLookChange",
        "LetterLabelSpacedroneIncoming",
        "LetterLabelRelicDestroyed",
        "LetterLabelRelicLost",
        "LetterLabelLastHackerLost",
        "LetterLabelRelicFound",
        "LetterLabelRelicsCollected",
        "LetterLabelPawnConnected",
        "LetterLabelConnectedTreeDestroyed",
        "LetterLabelArchonexusWealthReached",
        "LetterLabelArchonexusStructureResearched",
        "LetterLabelArchotechStructuresAbandoned",
        "LetterLabelReformIdeo",
        "LetterTitleObligationsActivated",
        "LetterTitleObligationsDeactivated",
        // BIOTECH
        "LetterLabelMechsFeral",
        "LetterLabelMechlinkInstalled",
        "LetterLabelBossgroupSummoned",
        "LetterLabelBossgroupArrived",
        "LetterLabelCommsConsoleSpawned",
        "LetterLabelBossgroupCallerUnlocked",
        "LetterLabelMechanitorCasketOpened",
        "LetterLabelMechanitorCasketFound",
        "LetterLabelBecameChild",
        "LetterLabelBecameAdult",
        "LetterLabelAdopted",
        "LetterLabelThirdTrimester",
        "LetterVatBirth",
        "LetterColonistPregnancyLaborLabel",
        "BabyAlreadyNamed",
        "BirthdayGrowthMoment",
        "LetterLabelMechlinkAvailable",
        "LetterLabelXenogermOrderedImplanted",
        "LetterLabelInvoluntaryDeathrest",
        "LetterLabelSanguophageWaitingToReimplant",
        "LetterLabelGenesImplanted",
        // CORE
        // Gatherings.xml 
        "LetterLabelParty",
        "LetterLabelMarriage",
        // Hediffs_Global_Misc.xml
        "LetterLabelPregnant",
        "LetterLabelOverdose",
        "LetterLabelCatatonicBreakdown",
        // HediffGiverSets.xml
        "LetterLabelSavant",
        // Inspirations.xml
        "LetterLabelInspiredWorkFrenzy",
        "LetterLabelInspiredGoFrenzy",
        "LetterLabelInspiredShootFrenzy",
        "LetterLabelInspiredTrade",
        "LetterLabelInspiredRecruitment",
        "LetterLabelInspiredTaming",
        "LetterLabelInspiredSurgery",
        "LetterLabelInspiredCreativity",
        // MentalStateDefs
        "LetterLabelBerserk",
        "LetterLabelArson",
        "LetterLabelJailbreaker",
        "LetterLabelSlaughter",
        "LetterLabelMurderous",
        "LetterLabelGaveUp",
        "LetterLabelPsyWander",
        "LetterLabelTantrum",
        "LetterLabelSadistic",
        "LetterLabelCorpse",
        "LetterLabelGlutton",
        "LetterLabelSadWander",
        "LetterLabelHiding",
        "LetterLabelInsulting",
        "LetterLabelConfused",
        "LetterLabelFleeing",
        "LetterLabelManhunting",
        // QuestScriptDefs
        "LetterLabelQuestExpired",
        "LetterLabelQuestFailed",
        "LetterLabelPaymentArrived",
        "LetterLabelChasing",
        // SurgeryOutcomeEffectDefs.xml
        "LetterLabelSurgeryFailed",
        "LetterLabelSurgeryFailedSterilized",
        // Storyteller
        "LetterLabelCaravanAmbush",
        "LetterLabelCaravanManhunter",
        "LetterLabelDiseaseFlu",
        "LetterLabelDiseasePlague",
        "LetterLabelDiseaseMalaria",
        "LetterLabelDiseaseSleeping",
        "LetterLabelDiseaseFibrous",
        "LetterLabelDiseaseSensory",
        "LetterLabelDiseaseGutWorms",
        "LetterLabelDiseaseMuscle",
        "LetterLabelDiseaseAnimalFlu",
        "LetterLabelDiseaseAnimalPlague",
        "LetterLabelAmbrosiaSprout",
        "LetterLabelRansom",
        "LetterLabelMeteorite",
        "LetterLabelAnimalMigration",
        "LetterLabelWanderer",
        "LetterLabelToxicFallout",
        "LetterLabelVolcanicWinter",
        "LetterLabelHeatWave",
        "LetterLabelColdSnap",
        "LetterLabelFlashstorm",
        "LetterLabelManInBlack",
        "LetterLabelInfestation",
        "LetterLabelDrillInfestation",
        "LetterLabelDefoliatorShip",
        "LetterLabelPsychicShip",
        "LetterLabelMechCluster",
        "LetterLabelEclipse",
        "LetterLabelPsychicDrone",
        "LetterLabelPsychicSoothe",
        "LetterLabelSolarFlare",
        "LetterLabelAurora",
        "LetterLabelQuestAvailable",
        "LetterLabelJourneyOffer",
        "LetterLabelRaidEnemy",
        "LetterLabelRaidFriendly",
        "LetterLabelRaidSiege",
        // Incidents.xml
        "LetterLabelAnimalInsanity",
        "LetterLabelCropBlight",
        "LetterLabelMiracleHeal",
        "LetterLabelRefugeePodCrash",
        "LetterLabelWandererJoins",
        "LetterLabelCargoPodCrash",
        "LetterLabelSingleVisitorArrives",
        "LetterLabelGroupVisitorsArrive",
        "LetterLabelManhunterPackArrived",
        "LetterLabelPsychicDroneLevelIncreased",
        "LetterLabelAgentRevealed",
        "LetterLabelBeaversArrived",
        "LetterLabelAnimalSelfTame",
        "LetterLabelFarmAnimalsWanderIn",
        "LetterLabelThrumboPasses",
        "LetterLabelTraderCaravanArrival",
        "EscapeShipFoundLabel",
        "LetterLabelCaravanRequest",
        "LetterLabelSiteNoLongerHostile",
        "LetterLabelSiteNoLongerHostileMulti",
        "LetterLabelQuestFailedReason",
        // Messages.xml
        "LetterLabelMessageRecruitSuccess",
        // Thoughts_Memory_Misc.xml
        "LetterLabelCatharsis",
        // ROYALTY
        // Gatherings.xml
        "LetterLabelConcert",
        // Incidents_Map_Special.xml
        "LetterLabelAnimaTree",
        // QuestScriptDefs
        "LetterLabelMonumentCompleted",
        "LetterLabelMonumentDestroyed",
        "LetterLabelMonumentMarkerArrived",
        "LetterLabelTimeExpired",
        "LetterLabelDecreeForgotten",
        "LetterLabelDecreeCancelled",
        "LetterLabelArrived",
        "LetterLabelGuestDied",
        "LetterLabelSubjectUnhappy",
        "LetterLabelGuestCaptured",
        "LetterLabelUnauthorizedSurgery",
        "LetterLabelGuestLost",
        "LetterLabelGuestUnhappy",
        "LetterLabelGuestCapture",
        "LetterLabelLoyaltySquad",
        "LetterLabelAngryAnimal",
        "LetterLabelCaptured",
        "LetterLabelChasingSubject",
        "LetterLabelSiteHarass",
        "LetterLabelSiteAppeared",
        "LetterLabelHeirFailed",
        "LetterLabelTitleHolderDied",
        "LetterLabelDesignatedHeirDied",
        "LetterLabelNewHeir",
        "LetterLabelDiedAnger",
        "LetterLabelDiedRevenge",
        "LetterLabelDiedDeparture",
        "LetterLabelArrestedAnger",
        "LetterLabelArrestedRevenge",
        "LetterLabelArrestedDeparture",
        "LetterLabelViolatedAnger",
        "LetterLabelViolatedRevenge",
        "LetterLabelViolatedDeparture",
        "LetterLabelBetrayal",
        "LetterLabelHospitalityReward",
        "LetterLabelBetrayalReward",
        "LetterLabelBetrayalRewardRetracted",
        "LetterLabelShuttleArrived",
        "LetterLabelShuttleDestroyed",
        "LetterLabelColonistsReturned",
        "LetterLabelRaidAnyone",
        "LetterLabelRescueShuttleArrived",
        "LetterLabelQuestCompleted",
        "LetterLabelLaborersArrived",
        "LetterLabelLaborerDied",
        "LetterLabelDestroyed",
        // Ritual_Behaviors.xml
        "LetterLabelThroneSpeech",
        // Incidents_Map_Disease.xml
        "LetterLabelDiseaseAbasia",
        "LetterLabelDiseaseBloodRot",
        // Incidents.xml
        "LetterLabelTributeCollectorArrival",
        // Misc_Gameplay.xml
        "LetterLabelLinkingRitualCompleted",
        // IDEOLOGY
        // Incidents_Map_Special.xml
        "LetterLabelBeggarsArrive",
        "LetterLabelPilgrimsArrive",
        "LetterLabelGauranlenPodSpawn",
        "LetterLabelInfestationJelly",
        "LetterLabelWanderersSkylantern",
        // MentalStates_Mood.xml
        "LetterLabelCrisisOfBelief",
        "LetterLabelRebel",
        // QuestScriptDefs
        "LetterLabelBeggarsBetrayed",
        "LetterLabelArchonexusDiscovered",
        // WorkSites.xml
        "LetterLabelWorkSite",
        // Incidents_World_Quests.xml
        "LetterLabelArchonexusVictory",
        // BIOTECH
        // HediffDefs
        "LetterLabelReusable",
        // Incidents_Map_Special.xml
        "LetterLabelAncientMechanitor",
        "LetterLabelPoluxTree",
        // MentalStates_Special.xml
        "LetterLabelFleeingFire",
        "LetterLabelWarcall",
        // QuestScriptDefs
        "LetterLabelBossDefeated",
        "LetterLabelMechanitorShip",
        "LetterLabelMechanoidAttack",
        "LetterLabelMechlinkAvailable",
        "LetterLabelWastepacks",
        "LetterLabelSanguophagesArrive",
        "LetterLabelSanguophagesArgument",
        "LetterLabelMeetingComplete",
        "LetterLabelSanguophagesHunters",
        "LetterLabelCrashedShuttle",
        "LetterLabelThrallReinforcements",
        "LetterLabelAssaultBegun",
        "LetterLabelAttached",
        // Incidents_Map_Threats.xml
        "LetterLabelWastepackInfestation",
        // ThingDefs_Buildings
        "LetterLabelAncientMech",
        "LetterLabelAvailable",
        // ANOMALY
        "LetterRevenantFleshChunkLabel",
        "LetterRevenantSeenLabel",
        "EmergenceWarningLabel",
        "MutatorObeliskLetterLabel",
        "ObeliskTentacleLetterLabel",
        "ObeliskFleshWhipLetterLabel",
        "MutatorObeliskFailedArmLetterLabel",
        "ObeliskFleshmassStomachLetterLabel",
        "MutatorObeliskFailedStomachLetterLabel",
        "ObeliskFleshmassLungLetterLabel",
        "MutatorObeliskFailedLungLetterLabel",
        "ObeliskAnimalMutationLetterLabel",
        "ObeliskTreeMutationLetterLabel",
        "ObeliskDuplicationLetterLabel",
        "ObeliskDuplicationFailedLetterLabel",
        "ObeliskHostileDuplicateLetterLabel",
        "DuplicatorObeliskLetterLabel",
        "ObeliskAbductorLetterLabel",
        "ObeliskAbductedDisappearingLetterLabel",
        "ObeliskAbductedLetterLabel",
        "MetalhorrorEmergingLabel",
        "LetterGrayFleshDiscoveredLabel",
        "LetterMetalhorrorReawakeningLabel",
        "LetterSurgicallyInspectedLabel",
        "LetterUnnaturalHealingLabel",
        "UnnaturalCorpseGuiLabel",
        "MonolithArrivalLabel",
        "MonolithAutoActivatingLabel",
        "MonolithAutoActivatedLabel",
        "StructuresActivatedAlertLabel",
        "LetterChimerasAttackingLabel",
        "VoidMonolithVisionLabel",
        "ReHumanizedLabel",
        "HateChantersLabel",
        "HateChantersAttackingLabel",
        "PsychicRitualCompleteLabel",
        "FleshmassResponseLabel",
        "VoidAwakeningStageOneStructuresActivatedLabel",
        "VoidAwakeningStageTwoStructuresActivatedLabel",
        "VoidAwakeningFinalStructuresActivatedLabel",
        "VoidAwakeningShamblerArrivalLabel",
        "VoidAwakeningFleshbeastBurrowLabel",
        "VoidAwakeningMetalhorrorArrivalLabel",
        "VoidAwakeningEntityArrivalLabel",
        "VoidAwakeningBreadcrumbLetterLabel",
        "VoidNodeReturnedLabel",
        "DuplicateSicknessLabel",
        "FrenziedAnimalsLabel",
        "GhoulBetrayalLabel",
        "DistressSignalLabel",
        "DistressSignalAmbushLabel",
        "ChimeraAssaultLabel",
        "DarknessLiftingEarlyLetterLabel",
        "PawnAttackedInDarknessLabel",
        "NoctolAttackLetterLabel",
        "DarknessWarningLetterLabel",
        "DarknessWaveringLetterLabel",
        "GhoulPodCrashLabel",
        "NociosphereDefeatedLabel",
        "NociosphereBecomingUnstableLabel",
        "NociosphereUnstableLabel",
        "UnnaturalCorpseArrivalLabel",
        "GoldenCubeArrivalLabel",
        "VoidCuriosityLabel"
    ];

    public List<string> ActiveMessagePatches
    {
        get
        {
            if (activeMessagePatches != null)
            {
                return activeMessagePatches;
            }

            activeMessagePatches = [];

            for (var i = 0; i < GenericMessageLabels.Count; i++)
            {
                if (GenericMessageValues[i])
                {
                    activeMessagePatches.Add(GenericMessageLabels[i]);
                }
            }

            return activeMessagePatches;
        }
        private set => activeMessagePatches = value;
    }

    public List<string> ActiveAlertPatches
    {
        set => activeAlertPatches = value;
    }

    public List<string> ActiveLetterPatches
    {
        get
        {
            if (activeLetterPatches != null)
            {
                return activeLetterPatches;
            }

            activeLetterPatches = [];

            for (var i = 0; i < GenericLetterLabels.Count; i++)
            {
                if (GenericLetterValues[i])
                {
                    activeLetterPatches.Add(GenericLetterLabels[i]);
                }
            }

            return activeLetterPatches;
        }
        private set => activeLetterPatches = value;
    }

    public override void ExposeData()
    {
        for (var i = 0; i < GenericMessageLabels.Count; i++)
        {
            Scribe_Values.Look(ref GenericMessageValues[i], GenericMessageLabels[i]);
        }

        for (var i = 0; i < GenericAlertLabels.Count; i++)
        {
            Scribe_Values.Look(ref GenericAlertValues[i], GenericAlertLabels[i]);
        }

        for (var i = 0; i < GenericLetterLabels.Count; i++)
        {
            Scribe_Values.Look(ref GenericLetterValues[i], GenericLetterLabels[i]);
        }

        Scribe_Values.Look(ref TaintedMessagePatch, "taintedMessagePatch");
        Scribe_Values.Look(ref IdleColonistsPatch, "idleColonistsPatch");
        Scribe_Values.Look(ref DrawAutoSelectCheckboxPatch, "drawAutoSelectCheckboxPatch");
        Scribe_Collections.Look(ref customFilters, "customFilters", LookMode.Value);
        Scribe_Collections.Look(ref seenText, "seenText", LookMode.Value);
        base.ExposeData();
    }

    public void ResetPatches()
    {
        ActiveMessagePatches = null;
        ActiveAlertPatches = null;
        ActiveLetterPatches = null;
    }

    public bool GetGenericAlertPatchValue(string label)
    {
        for (var i = 0; i < GenericAlertLabels.Count; i++)
        {
            if (GenericAlertLabels[i] == label)
            {
                return GenericAlertValues[i];
            }
        }

        BUMMod.Instance.Settings.TryAddSeenText(label);

        throw new ArgumentException($"Argument {label} not found in the list.");
    }

    public void TryAddSeenText(string text)
    {
        if (SeenText.Contains(text))
        {
            return;
        }

        SeenText.Add(text);

        if (SeenText.Count > 50)
        {
            // Trim it
            SeenText = SeenText.GetRange(SeenText.Count - 50, 50);
        }

        Scribe_Collections.Look(ref seenText, "seenText", LookMode.Value);
    }

    public bool ShouldBlock(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }

        foreach (var customFilter in CustomFilters)
        {
            if (Matches(customFilter, text))
            {
                return true;
            }
        }

        return false;
    }

    public static bool Matches(string filter, string text)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return false;
        }

        if (!filter.Contains("*"))
        {
            return filter.Equals(text, StringComparison.OrdinalIgnoreCase);
        }

        var filterSplit = filter.Split('*');
        var resultingArray = new List<string>();
        foreach (var part in filterSplit)
        {
            resultingArray.Add(Regex.Escape(part));
        }

        var resultingString = string.Join(".*", resultingArray);

        return Regex.IsMatch(text, resultingString, RegexOptions.IgnoreCase);
    }
}