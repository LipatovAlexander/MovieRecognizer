variable "folder_id" {
  type = string
}

variable "name" {
  type = string
}

variable "namespace" {
  type = string
}

variable "bucket" {
  type = string
}

variable "roles" {
  type = list(string)
}

variable "memory" {
  type    = string
  default = "128"
}

variable "execution_timeout" {
  default = 10
}

variable "environment" {
  type = map(string)
}

variable "deployer_access_key" {
  type = string
}

variable "deployer_secret_key" {
  type = string
}

variable "github_sha" {
  type = string
}

variable "max_retries" {
  type = number
}