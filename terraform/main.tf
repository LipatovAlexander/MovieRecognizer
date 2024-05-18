terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
    }

    random = {
      source  = "hashicorp/random"
      version = "3.6.1"
    }
  }

  backend "s3" {
    endpoints = {
      s3 = "https://storage.yandexcloud.net"
    }
    bucket = "movie-recognizer-terraform-state"
    region = "ru-central1"
    key    = "main.tfstate"

    skip_region_validation      = true
    skip_credentials_validation = true
    skip_requesting_account_id  = true
    skip_s3_checksum            = true
  }

  required_version = ">= 1.7.4"
}

provider "yandex" {
  zone = "ru-central1-a"
}