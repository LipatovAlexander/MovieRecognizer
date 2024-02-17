#!/bin/bash
echo "Creating AWS services..."

awslocal s3api create-bucket --bucket application

echo "AWS services have been successfully created."