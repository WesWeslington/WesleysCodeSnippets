 // Fill out your copyright notice in the Description page of Project Settings.


#include "CDoor.h"
#include "Components/StaticMeshComponent.h"
#include "Components/TimelineComponent.h"
#include "Components/SphereComponent.h"
#include "CCharacter.h"
// Sets default values
ACDoor::ACDoor()
{
	DoorFrameMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("DoorFrameMesh"));
	DoorFrameMesh->SetCollisionEnabled(ECollisionEnabled::QueryOnly);

	RootComponent = DoorFrameMesh;

	DoorMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("DoorMesh"));
	DoorMesh->AttachTo(DoorFrameMesh);

	CollisionSphere = CreateDefaultSubobject<USphereComponent>(TEXT("CollisionSphere"));
	CollisionSphere->AttachTo(DoorFrameMesh);
	
}

// Called when the game starts or when spawned
void ACDoor::BeginPlay()
{
	Super::BeginPlay();
	

	
}

void ACDoor::NotifyActorBeginOverlap(AActor* OtherActor)
{
	Super::NotifyActorBeginOverlap(OtherActor);

	ACCharacter* OtherPlayer = Cast<ACCharacter>(OtherActor);

	if (!bIsSlideDoor) {

		if (OtherPlayer && !bIsMoving) {

			FVector DoorLocation = GetActorLocation();
			FVector PlayerLocation = OtherPlayer->GetActorLocation();



			if (bIsHorizontal) {
				bPlayerIsInFront = (bool)(PlayerLocation.Y > DoorLocation.Y);
			}
			else {
				bPlayerIsInFront = (bool)(PlayerLocation.X > DoorLocation.X);

			}

			PlayerNearAnimation();
			bIsMoving = true;

			//in the blueprints, set IsMoving to false after the lerp
		}

	}
	else {

		if (OtherPlayer && !bIsMoving) {
			PlayerNearAnimation();
			bIsMoving = true;
			//in the blueprints, set IsMoving to false after the lerp
		}

	}
}

void ACDoor::NotifyActorEndOverlap(AActor* OtherActor)
{
	Super::NotifyActorEndOverlap(OtherActor);

	ACCharacter* OtherPlayer = Cast<ACCharacter>(OtherActor);

	if ((OtherPlayer && bIsOpen) && bIsSlideDoor) {

		PlayerExitDoorArea();
		bIsMoving = true;

	}

}