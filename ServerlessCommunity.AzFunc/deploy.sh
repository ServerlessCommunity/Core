#!/bin/bash

# az login -o table

az account set -s ce42ba48-ac45-41a1-b25a-2c869b398f71

az account list -o table

func azure functionapp publish azureuaserverlessprod 
