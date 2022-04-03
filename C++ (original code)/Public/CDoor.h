// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Pawn.h"
#include "CDoor.generated.h"

class UStaticMeshComponent;
class USphereComponent;
class UTimelineComponent;
UCLASS()
class WTCC_CRIMESIMULATION_API ACDoor : public APawn
{
	GENERATED_BODY()

public:
	// Sets default values for this pawn's properties
	ACDoor();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	void AnimateDoor();

public:	

	UPROPERTY(EditDefaultsOnly, Category = "Door")
	UTimelineComponent* DoorAnimation;

	UPROPERTY(BlueprintReadOnly, EditDefaultsOnly, Category = "Door")
	UStaticMeshComponent* DoorMesh;
	UPROPERTY(EditDefaultsOnly, Category = "Door")
	UStaticMeshComponent* DoorFrameMesh;

	UPROPERTY(EditDefaultsOnly, Category = "Door")
	USphereComponent* CollisionSphere;

	

	UFUNCTION(BlueprintImplementableEvent, Category = "Door")
	void PlayerNearAnimation();

	UFUNCTION(BlueprintImplementableEvent, Category = "Door")
		void PlayerExitDoorArea();

	virtual void NotifyActorBeginOverlap(AActor* OtherActor) override;
	virtual void NotifyActorEndOverlap(AActor* OtherActor) override;

	UPROPERTY(BlueprintReadWrite, Category = "Door")
	bool bIsMoving = false;

	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = "Door")
float DoorCooldown = 3.0f;

	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = "Door")
		bool bIsSlideDoor = false;

	UPROPERTY(BlueprintReadOnly, Category = "Door")
	bool bPlayerIsInFront;

	UPROPERTY(EditInstanceOnly, Category = "Door")
	bool bIsHorizontal;

	UPROPERTY(BlueprintReadWrite, Category = "Door")
		bool bIsOpen;

};
