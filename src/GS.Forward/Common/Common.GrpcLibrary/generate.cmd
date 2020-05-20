protoc -I . --csharp_out ./Generated/Lib --grpc_out ./Generated/Grpc --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe Protos/*.proto
pause