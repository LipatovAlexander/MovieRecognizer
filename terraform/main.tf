terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
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

resource "yandex_storage_bucket" "test" {
  bucket = "movie-recognizer-test"
}