#Project Variables
PROJECT_NAME ?= SolarCoffee2
ORG_NAME ?= SolarCoffee2
REPO_NAME ?= SolarCoffee2

.PHONY: migrations db

migrations:
	cd ./SolarCoffee.Data &&  dotnet ef --startup-project ../SolarCoffee.Web/ migrations add $(mname) && cd ..

db:
	cd ./SolarCoffee.Data &&  dotnet ef --startup-project ../SolarCoffee.Web/ database update && cd ..
