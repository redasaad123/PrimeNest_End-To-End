terraform {
  backend "s3" {
    bucket = "primenest-terraform-state"
    key    = "node/terraform.tfstate"
    region = "eu-central-1"
    encrypt = true
    
  }


  required_providers {
    aws ={
        source = "hashicorp/aws"
        version = "~> 5.0"
    }
  }

  required_version = ">= 1.0.0"
}